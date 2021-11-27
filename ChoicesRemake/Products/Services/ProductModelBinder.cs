using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Products.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Products.Services
{
    public class ProductModelBinder : IModelBinder
    {
        private ILogger logger;

        public ProductModelBinder(ILogger<ProductModelBinder> logger)
        {
            this.logger = logger;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null || bindingContext.ModelType != typeof(Product))
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            bindingContext.Result = ModelBindingResult.Failed();

            try
            {
                var modelMetadata = bindingContext.ModelMetadata.GetMetadataForProperties(typeof(Product));
                var modelNames = new List<string>();
                foreach (var property in modelMetadata)
                {
                    if (property != null && property.Name != null)
                    {
                        modelNames.Add(property.Name);
                    }
                    else
                    {
                        return Task.CompletedTask;
                    }
                }
                var product = new Product();
                foreach (var modelName in modelNames)
                {
                    var name = modelName.ToLower();
                    if (name == "images")
                    {
                        var files = bindingContext.HttpContext.Request.Form.Files;
                        if (files == null || files.Count == 0)
                        {
                            return Task.CompletedTask;
                        }

                        foreach (var file in files)
                        {
                            if (file != null)
                            {
                                product.images.Add(file);
                            }
                        }

                        continue;
                    }

                    var valueProviderResult = bindingContext.ValueProvider.GetValue(name);

                    if (valueProviderResult == ValueProviderResult.None)
                    {
                        return Task.CompletedTask;
                    }

                    var value = valueProviderResult.FirstValue;

                    if (value == null)
                    {
                        return Task.CompletedTask;
                    }
                    if (name == "brand")
                    {
                        product.Brand = value;
                    }
                    else if (name == "designer")
                    {
                        product.Designer = value;
                    }
                    else if (name == "name")
                    {
                        product.Name = value;
                    }
                    else if (name == "price")
                    {
                        var parseResult = decimal.TryParse(value, out decimal price);
                        if (!parseResult)
                        {
                            return Task.CompletedTask;
                        }
                        product.Price = price;
                    }
                    else if (name == "descriptions")
                    {
                        var desc = JsonSerializer.Deserialize<List<Description>>(value);
                        if (desc == null)
                        {
                            return Task.CompletedTask;
                        }
                        product.descriptions = desc;
                    }
                    else if (name == "masses")
                    {
                        var mass = JsonSerializer.Deserialize<List<Mass>>(value);
                        if (mass == null)
                        {
                            return Task.CompletedTask;
                        }
                        product.masses = mass;
                    }
                    else if (name == "colors")
                    {
                        var colors = JsonSerializer.Deserialize<List<string>>(value);
                        if (colors == null)
                        {
                            return Task.CompletedTask;
                        }
                        product.colors = colors;
                    }
                    else if (name == "categories")
                    {
                        var categories = JsonSerializer.Deserialize<List<string>>(value);
                        if (categories == null)
                        {
                            return Task.CompletedTask;
                        }
                        product.categories = categories;
                    }
                    else if (name == "sizes")
                    {
                        var sizes = JsonSerializer.Deserialize<List<Size>>(value);
                        if (sizes == null)
                        {
                            return Task.CompletedTask;
                        }
                        product.sizes = sizes;
                    }
                    else if (name == "details")
                    {
                        var details = JsonSerializer.Deserialize<List<MiscDetail>>(value);
                        if (details == null)
                        {
                            return Task.CompletedTask;
                        }
                        product.details = details;
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(product);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                logger.LogError($"Model Binding for Product failed with {e.Message}");
                return Task.CompletedTask;
            }
        }
    }
}