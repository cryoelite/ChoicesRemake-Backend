using DiffMatchPatch;
using IProductsRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsDBLayer;
using ProductsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsRepository
{
    public class ProductRepo : IProductRepo
    {
        private ILogger<ProductRepo> logger;
        private ProductsDBContext pdb;

        public ProductRepo(ProductsDBContext dbContext, ILogger<ProductRepo> logger) => (pdb, this.logger) = (dbContext, logger);

        public async Task<List<Category>?> getCategories(Product product)
        {
            var db = await pdb.Categories.AsNoTracking().Where(x => x.CatId == product.CatId).Take(15).ToListAsync();
            return db;
        }

        public async Task<List<Color>?> getColors(Product product)
        {
            var db = await pdb.Colors.AsNoTracking().Where(x => x.ColorId == product.ColorId).Take(15).ToListAsync();
            return db;
        }

        public async Task<List<Description>?> getDescriptions(Product product)
        {
            var db = await pdb.Descriptions.AsNoTracking().Where(x => x.DescId == product.DescId).Take(15).ToListAsync();
            return db;
        }

        public async Task<List<MiscDetail>?> getDetails(Product product)
        {
            var db = await pdb.MiscDetails.AsNoTracking().Where(x => x.DetailId == product.DetailId).Take(15).ToListAsync();
            return db;
        }

        public async Task<List<Image>?> getImages(Product product)
        {
            var db = await pdb.Images.AsNoTracking().Where(x => x.ImageId == product.ImageId).Take(15).ToListAsync();
            return db;
        }

        public async Task<List<Mass>?> getMasses(Product product)
        {
            var db = await pdb.Masses.AsNoTracking().Where(x => x.MassId == product.MassId).Take(15).ToListAsync();
            return db;
        }

        public async Task<Product?> getProductById(int id)
        {
            logger.LogInformation("Getting product by ID");

            var product = await pdb.Products.FindAsync(id);
            if (product == null)
            {
                logger.LogInformation($"Product with id {id} not found");

                return null;
            }
            logger.LogInformation($"Product with id {id} successfully found");
            product = await initiaLizeProduct(product);
            return product;
        }

        public async Task<List<Size>?> getSizes(Product product)
        {
            var db = await pdb.Sizes.AsNoTracking().Where(x => x.SizeId == product.SizeId).Take(15).ToListAsync();
            return db;
        }

        public async Task<List<Product>?> searchAndGetProductsByName(string name)
        {
            logger.LogInformation($"Searching for products with name {name}");

            var dmp = new diff_match_patch();

            var products = pdb.Products.AsNoTracking().Where(delegate (Product product)
           {
               var convName = name.Trim().ToLower().Replace(" ", "");
               var convProdName = product.Name.Trim().ToLower().Replace(" ", "");
               var diff = dmp.diff_main(convName, convProdName);
               var result = dmp.diff_levenshtein(diff);

               double similarity = 100 - ((double)result / Math.Max(convName.Length, convProdName.Length) * 100);
               return similarity >= 60;
           }).Take(15).ToList();
            logger.LogInformation($"Product with name {name} {(products == null ? "not" : "")} found");
            if (products != null)
            {
                var productList = new List<Product>();

                foreach (var product in products)
                {
                    productList.Add(await initiaLizeProduct(product));
                }
                return productList;
            }
            return null;
        }

        public async Task<List<Product>?> searchAndGetProductsByPriceRange(decimal minPrice = decimal.MinValue, decimal maxPrice = decimal.MaxValue)
        {
            logger.LogInformation($"Searching for within price {minPrice} and {maxPrice}");

            var products = await pdb.Products.AsNoTracking().Where(product => product.Price >= minPrice && product.Price < maxPrice).ToListAsync();
            logger.LogInformation($"Found {products.Count} products");

            if (products != null)
            {
                var productList = new List<Product>();

                foreach (var product in products)
                {
                    productList.Add(await initiaLizeProduct(product));
                }
                return productList;
            }
            return null;
        }

        public async Task storeProduct(Product product)
        {
            logger.LogInformation($"Storing a new product with name {product.Name}");

            foreach (var cat in product.local_categories)
            {
                var _product = new Product();
                cat.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = product.local_colors[0];
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = cat;

                _product.Detail = product.local_details[0];

                _product.Size = product.local_sizes[0];
                _product.Mass = product.local_masses[0];
                _product.Image = product.local_images[0];
                _product.Desc = product.local_descs[0];
                await pdb.AddAsync(cat);
                await pdb.AddAsync(_product);
            }
            foreach (var color in product.local_colors)
            {
                var _product = new Product();
                color.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = color;
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = product.local_categories[0];

                _product.Detail = product.local_details[0];

                _product.Size = product.local_sizes[0];
                _product.Mass = product.local_masses[0];
                _product.Image = product.local_images[0];
                _product.Desc = product.local_descs[0];
                await pdb.AddAsync(color);
                await pdb.AddAsync(_product);
            }
            foreach (var image in product.local_images)
            {
                var _product = new Product();
                image.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = product.local_colors[0];
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = product.local_categories[0];

                _product.Detail = product.local_details[0];

                _product.Size = product.local_sizes[0];
                _product.Mass = product.local_masses[0];
                _product.Image = image;
                _product.Desc = product.local_descs[0];
                await pdb.AddAsync(image);
                await pdb.AddAsync(_product);
            }
            foreach (var mass in product.local_masses)
            {
                var _product = new Product();
                mass.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = product.local_colors[0];
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = product.local_categories[0];

                _product.Detail = product.local_details[0];

                _product.Size = product.local_sizes[0];
                _product.Mass = mass;
                _product.Image = product.local_images[0];
                _product.Desc = product.local_descs[0];
                await pdb.AddAsync(mass);
                await pdb.AddAsync(_product);
            }

            foreach (var detail in product.local_details)
            {
                var _product = new Product();
                detail.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = product.local_colors[0];
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = product.local_categories[0];

                _product.Detail = detail;

                _product.Size = product.local_sizes[0];
                _product.Mass = product.local_masses[0];
                _product.Image = product.local_images[0];
                _product.Desc = product.local_descs[0];
                await pdb.AddAsync(detail);
                await pdb.AddAsync(_product);
            }

            foreach (var size in product.local_sizes)
            {
                var _product = new Product();
                size.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = product.local_colors[0];
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = product.local_categories[0];

                _product.Detail = product.local_details[0];

                _product.Size = size;
                _product.Mass = product.local_masses[0];
                _product.Image = product.local_images[0];
                _product.Desc = product.local_descs[0];
                await pdb.AddAsync(size);
                await pdb.AddAsync(_product);
            }
            foreach (var desc in product.local_descs)
            {
                var _product = new Product();
                desc.Products.Add(_product);

                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.Color = product.local_colors[0];
                _product.Brand = product.Brand;
                _product.Designer = product.Designer;
                _product.Cat = product.local_categories[0];

                _product.Detail = product.local_details[0];

                _product.Size = product.local_sizes[0];
                _product.Mass = product.local_masses[0];
                _product.Image = product.local_images[0];
                _product.Desc = desc;
                await pdb.AddAsync(desc);
                await pdb.AddAsync(_product);
            }
            await pdb.SaveChangesAsync();
            logger.LogInformation($"Successfully stored product with name {product.Name}");
        }

        private async Task<Product> initiaLizeProduct(Product product)
        {
            var cats = await getCategories(product);
            var details = await getDetails(product);
            var sizes = await getSizes(product);
            var images = await getImages(product);
            var colors = await getColors(product);
            var descriptions = await getDescriptions(product);
            var masses = await getMasses(product);

            if (cats != null)
            {
                product.local_categories.AddRange(cats);
            }
            if (details != null)
            {
                product.local_details.AddRange(details);
            }
            if (sizes != null)
            {
                product.local_sizes.AddRange(sizes);
            }
            if (images != null)
            {
                product.local_images.AddRange(images);
            }
            if (colors != null)
            {
                product.local_colors.AddRange(colors);
            }
            if (descriptions != null)
            {
                product.local_descs.AddRange(descriptions);
            }
            if (masses != null)
            {
                product.local_masses.AddRange(masses);
            }

            return product;
        }
    }
}