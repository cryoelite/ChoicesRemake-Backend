// Custom input formatter, incomplete and retired.

/*using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Products.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LocalProduct = Products.Models.Product;

namespace Products.Services
{
    public class ImageTypeConverter : TextInputFormatter
    {
        public ImageTypeConverter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(LocalProduct);
        }

        private BaseImage ProcessImages(JsonElement elem)
        {
            ImageData? byteImage = null;
            ImageURL? urlImage = null;
            string? miniDesc = null;
            string? name = null;
            string? url = null;
            byte[]? byteData = null;
            if (elem.TryGetProperty("miniDesc", out JsonElement desc))
            {
                miniDesc = desc.GetString();
            }
            if (elem.TryGetProperty("name", out JsonElement _name))
            {
                name = _name.GetString();
            }

            if (miniDesc == null || name == null)
            {
                throw new Exception("Image Data incorrect in json, missing miniDesc or name");
            }
            if (elem.TryGetProperty("location", out JsonElement _url))
            {
                url = _url.GetString();
            }
            if (elem.TryGetProperty("data", out JsonElement _data))
            {
                var dataArray = _data.GetBytesFromBase64();
                byteData = dataArray;
            }

            if (url == null && byteData == null)
            {
                throw new Exception("Image Data incorrect in json, missing url or data");
            }

            if (url != null)
            {
                urlImage = new ImageURL(miniDesc, name, url);
            }
            if (byteData != null)
            {
                byteImage = new ImageData(miniDesc, name, byteData);
            }

            if (byteImage != null)
            {
                return byteImage;
            }
            else
            {
                return urlImage!;
            }
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<ImageTypeConverter>>();
            using var reader = new StreamReader(httpContext.Request.Body, encoding);
            {
                var data = await reader.ReadToEndAsync();

                using (JsonDocument doc = JsonDocument.Parse(data))
                {
                    try
                    {
                        var root = doc.RootElement;
                        var images = root.GetProperty("images");
                        var imagesList = new List<BaseImage>();
                        foreach (var elem in images.EnumerateArray())
                        {
                            var result = ProcessImages(elem);
                            imagesList.Add(result);
                        }
                        var brand = root.GetProperty("brand");

                        return await InputFormatterResult.SuccessAsync("");
                    }
                    catch(Exception e)
                    {
                        logger.LogError(e.Message);
                        return await InputFormatterResult.FailureAsync();
                    }
                }
            }
        }
    }
}
*/