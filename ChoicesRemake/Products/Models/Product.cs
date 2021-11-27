using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services;
using System.Collections.Generic;

namespace Products.Models
{
    [ModelBinder(typeof(ProductModelBinder))]
    public class Product
    {
        public Product(string Brand, string Designer, string Name, decimal Price, List<Description> descriptions, List<Mass> masses, List<string> colors,
            List<string> categories, List<Size> sizes, List<IFormFile> images, List<MiscDetail> details)
        {
            this.Brand = Brand;
            this.Designer = Designer;
            this.Name = Name;
            this.Price = Price;
            this.descriptions = descriptions;
            this.masses = masses;
            this.colors = colors;
            this.categories = categories;
            this.sizes = sizes;
            this.images = images;
            this.details = details;
        }

        public Product()
        { }

        [ModelBinder(Name = "brand")]
        public string Brand { get; set; } = string.Empty;

        [ModelBinder(Name = "categories")]
        public List<string> categories { get; set; } = new List<string>();

        [ModelBinder(Name = "colors")]
        public List<string> colors { get; set; } = new List<string>();

        [ModelBinder(Name = "descriptions")]
        public List<Description> descriptions { get; set; } = new List<Description>();

        [ModelBinder(Name = "designer")]
        public string Designer { get; set; } = string.Empty;

        [ModelBinder(Name = "details")]
        public List<MiscDetail> details { get; set; } = new List<MiscDetail>();

        [ModelBinder(Name = "images")]
        public List<IFormFile> images { get; set; } = new List<IFormFile>();

        [ModelBinder(Name = "masses")]
        public List<Mass> masses { get; set; } = new List<Mass>();

        [ModelBinder(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [ModelBinder(Name = "price")]
        public decimal Price { get; set; } = decimal.Zero;

        [ModelBinder(Name = "sizes")]
        public List<Size> sizes { get; set; } = new List<Size>();
    }
}