using IProductsRepository;
using KafkaService.Models;
using KafkaService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nClam;
using Products.Services;
using ProductsModel;
using StaticAssets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using LocalProduct = Products.Models.Product;


namespace Products.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Products : ControllerBase
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignature =
        new Dictionary<string, List<byte[]>>
{
        { ".jpeg", new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
        }
    },{ ".png", new List<byte[]> {
        new byte[] {0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A  },
    }
            },
        { ".jpg", new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
        }
    },
};

        private readonly KafkaProducer kafkaProducer;
        private ClamClient clam;
        private ILogger<Products> logger;
        private IProductRepo productsRepository;

        public Products(IProductRepo productRepo, ILogger<Products> logger, KafkaProducer producer, ClamClient clamClient)
        {
            productsRepository = productRepo;
            this.logger = logger;
            this.kafkaProducer = producer;
            this.clam = clamClient;
        }

        [HttpGet("SearchItem")]
        public async Task<IActionResult> SearchItem([FromQuery(Name = "q")] string searchTerm)
        {
            var products = await productsRepository.searchAndGetProductsByName(searchTerm);
            if (products != null)
            {
                var productList = new List<dynamic>();
                var distinctProducts = products.GroupBy(x => x.Name).Select(y => y.First());
                foreach (var localProduct in distinctProducts)
                {
                    var item = new
                    {
                        Name = localProduct.Name,
                        Price = localProduct.Price,
                        Brand = localProduct.Brand,
                        Designer = localProduct.Designer,
                        Categories = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_categories.Select(x => x.Value).First()).Distinct(),
                        Descriptions = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_descs.Select(x => new { Title = x.Title, Desc = x.LongDescription }).First()).Distinct(),
                        Images = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_images.Select(x => new { Name = x.Name, Desc = x.MiniDesc, URL = x.Location }).First()).Distinct(),
                        Misc_Details = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_details.Select(x => new { Key = x.Key, Value = x.Value }).First()).Distinct(),
                        Masses = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_masses.Select(x => x.MassInKg).First()).Distinct(),
                        Sizes = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_sizes.Select(x => new { Width = x.WidthInMm, Height = x.HeightInMm, Length = x.LengthInMm }).First()).Distinct(),
                        Colors = products.Where(product => product.Name == localProduct.Name).Select(product => product.local_colors.Select(x => x.Value).First()).Distinct(),
                    };

                    productList.Add(item);
                }

                var jsonResult = JsonSerializer.Serialize(productList);
                return Ok(jsonResult);
            }
            return BadRequest("No product with given name found");
        }


        [Consumes("multipart/form-data")]
        [HttpPost("StoreItem")]
        public async Task<IActionResult> StoreItemData([FromForm] LocalProduct product)
        {
            logger.LogInformation("StoreItemData called");
            if (ModelState.IsValid)
            {
                logger.LogInformation("ModelState is Valid!");

                try
                {
                    var Storeproduct = new Product()
                    {
                        Brand = product.Brand,
                        Name = product.Name,
                        Designer = product.Designer,
                        Price = product.Price,
                    };

                    var cats = product.categories.Select(x => new Category() { Value = x });
                    var colors = product.colors.Select(x => new Color() { Value = x });
                    var descs = product.descriptions.Select(x => new Description() { LongDescription = x.LongDescription, Title = x.Title });
                    var size = product.sizes.Select(x => new Size() { WidthInMm = x.WidthInMm, LengthInMm = x.LengthInMm, HeightInMm = x.HeightInMm });
                    var mass = product.masses.Select(x => new Mass() { MassInKg = x.MassInKg });
                    var detail = product.details.Select(x => new MiscDetail() { Key = x.Key, Value = x.Value });
                    Storeproduct.local_descs.AddRange(descs);
                    Storeproduct.local_categories.AddRange(cats);

                    Storeproduct.local_sizes.AddRange(size);

                    Storeproduct.local_details = new List<MiscDetail>();
                    Storeproduct.local_details.AddRange(detail);

                    Storeproduct.local_masses.AddRange(mass);
                    Storeproduct.local_colors.AddRange(colors);
                    if (cats.Count() == 0 || colors.Count() == 0 || descs.Count() == 0 || size.Count() == 0 || mass.Count() == 0
                        || detail.Count() == 0 || product.Brand == String.Empty || product.Designer == String.Empty || product.Name == String.Empty || product.Price == decimal.Zero
                        )
                    {
                        return BadRequest("Please provide all required data.");
                    }

                    List<Image> images = await processImages(product.Brand, product.images);
                    logger.LogInformation("Images Processed");

                    Storeproduct.local_images.AddRange(images);
                    if (images.Count() == 0)
                    {
                        return BadRequest("Please provide atleast 1 valid image.");
                    }
                    logger.LogInformation("Storing Data in repo");

                    await productsRepository.storeProduct(Storeproduct);
                    logger.LogInformation("Data stored in repo sucessfully");

                    return Ok();
                }
                catch (Exception e)
                {
                    logger.LogError($"Error in storing product. The issue is "+ e.Message);

                    return Problem(e.Message, statusCode: 500);
                }
            }

            return BadRequest("Invalid request body");
        }

        [NonAction]
        private async Task<List<Image>> processImages(string Brand, List<IFormFile> images)
        {
            logger.LogInformation("Starting Image Processing");

            var imagesConv = new List<Image>();
            foreach (var item in images)
            {
                if (item.Length > 0 && (item.ContentType == "image/jpeg" || item.ContentType == "image/png"))
                {
                    var imageType = string.Concat(".", item.ContentType.AsSpan(item.ContentType.LastIndexOf('/') + 1));

                    //or using (var stream = System.IO.File.Create(filePath, 131072, FileOptions.DeleteOnClose)) where file is stored first.
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        var data = stream.ToArray();
                        logger.LogInformation("Sending clam av the file to be scanned.");

                        var scanResult = await clam.SendAndScanFileAsync(data);
                        logger.LogInformation("File scanned by clamAV");

                        if (scanResult != null)
                        {
                            switch (scanResult.Result)
                            {
                                case ClamScanResults.Clean:
                                    logger.LogInformation($"{item.FileName} scanned, it is clean");

                                    var signDBResult = _fileSignature.TryGetValue(imageType, out List<byte[]>? signatures);
                                    if (!signDBResult || signatures == null)
                                    {
                                        throw new Exception($"{item.FileName} Corrupt Image Detected, Please upload correct type image, i.e., jpeg or png");
                                    }
                                    var headerByes = data.Take(signatures.Max(m => m.Length));
                                    var signResult = signatures.Any(signature => headerByes.Take(signature.Length).SequenceEqual(signature));
                                    if (!signResult)
                                    {
                                        throw new Exception($"{item.FileName} Corrupt Image Detected, Please upload correct type image, i.e., jpeg or png");
                                    }

                                    var kafkaData = new KafkaData(InvocationType.invokeAndReturn, MethodNames.storeImageAndGetPath);
                                    kafkaData.AddRawHeader(CustomHeader.imageKey, data);
                                    kafkaData.AddHeader(CustomHeader.fileType, imageType);
                                    var brandRegex = Regex.Matches(Brand, @"[A-Za-z]");
                                    string brandValue = Path.GetRandomFileName();
                                    if (brandRegex != null && brandRegex.Count > 0)
                                    {
                                        var sb = new StringBuilder();
                                        var brandRegexElements = brandRegex.ToArray();
                                        foreach (var elem in brandRegexElements)
                                        {
                                            sb.Append(elem.Value.ToString());
                                        }
                                        brandValue = sb.ToString();
                                    }
                                    kafkaData.AddHeader(CustomHeader.fileBrand, brandValue);

                                    var result = await kafkaProducer.SendAndReceiveData(kafkaData);
                                    if (result.message.Value != ResultStatus.success)
                                    {
                                        logger.LogError($"Error in storing image. The result is: "+ result.message.Value);

                                        throw new Exception($"Internal server error in processing the request");
                                    }
                                    var imageLocation = result.GetCustomHeader(CustomHeader.imageLocation);
                                    if (imageLocation == null)
                                    {
                                        logger.LogError($"Error in getting image location");
                                        throw new Exception($"Internal server error in processing the request");
                                    }

                                    var image = new Image() { Location = imageLocation, Name = HttpUtility.HtmlEncode(item.FileName), MiniDesc = "" };
                                    imagesConv.Add(image);
                                    break;

                                case ClamScanResults.VirusDetected:
                                    logger.LogError($"{item.FileName} has Virus");
                                    throw new Exception($"{item.FileName} virus detected");
                                case ClamScanResults.Error:
                                    logger.LogError($"Error in scanning { item.FileName}");
                                    throw new Exception($"Error in scanning { item.FileName}");
                                default:
                                    logger.LogError($"Unknown error after scanning { item.FileName}");
                                    throw new Exception($"Unknown error after scanning { item.FileName}");
                            }
                        }
                    }
                }
            }
            return imagesConv;
        }
    }
}