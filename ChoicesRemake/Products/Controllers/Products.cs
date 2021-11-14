﻿using IProductsRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsDBLayer;
using ProductsRepository;
using System.Text.Json;
using System.Threading.Tasks;

namespace Products.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Products : ControllerBase
    {
        private IProductRepo productsRepository;
        private ILogger<Products> logger;
        public Products(IProductRepo productRepo, ILogger<Products> logger)
        {
            productsRepository= productRepo;
            this.logger= logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            /* var product = new Product()
             {
                 Brand = "IMPLEMENTS",
                 Name = "Standard Plate",
                 Designer = "Masanori Daiji",
                 Price = 4290,
             };

             var cat = new Category()
             {
                 Value = "LivingWare",
             };

             var color = new Color()
             {
                 Value = BaseColor.Blue,
             };

             var desc = new Description()
             {
                 Title = "xyz",
                 LongDescription = "Something"
             };

             var img = new Image()
             {
                 Location = "nooo",
                 MiniDesc = "yoo",
                 Name = "Standard Plate",
             };

             var mass = new Mass()
             {
                 MassInKg = 0,
             };

             var miscDetail = new MiscDetail()
             {
                 Key = "Remarks",
                 Value = "Yahoo",
             };

             var size = new Size()
             {
                 WidthInMm = 200,
                 HeightInMm = 28,
                 LengthInMm = 0,
             };

             await productInterface.storeProduct(product, cat, color, desc, img, mass, miscDetail, size);*/
            var product = productsRepository.searchAndGetProductsByName("standard plate");
            var jsonProduct = JsonSerializer.Serialize(product);
            return Ok(jsonProduct);
        }
    }
}