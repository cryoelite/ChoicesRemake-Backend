using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiffMatchPatch;
using IProductsRepository;
using Microsoft.EntityFrameworkCore;
using ProductsDBLayer;
using ProductsModel;

namespace ProductsRepository
{
    public class ProductRepo : IProductRepo
    {
        ProductsDBContext pdb;
        public ProductRepo(ProductsDBContext dbContext) => (pdb) = (dbContext);
#nullable enable
        public async Task<Product?> getProductById(int id)
        {
            var product = await pdb.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            return product;
        }
#nullable disable
        public async Task<List<Product>> searchAndGetProductsByName(string name)
        {

            var dmp = new diff_match_patch();


            var products = await pdb.Products.AsNoTracking().Where(delegate (Product product)
            {
                var convName = name.Trim().ToLower().Replace(" ", "");
                var convProdName = name.Trim().ToLower().Replace(" ", "");
                var diff = dmp.diff_main(convName, convProdName);
                var result = dmp.diff_levenshtein(diff);

                double similarity = 100 - ((double)result / Math.Max(convName.Length, convProdName.Length) * 100);
                return similarity >= 60;

            }).AsQueryable().ToListAsync();


            return products;
        }

        public async Task<List<Product>> searchAndGetProductsByPriceRange(decimal minPrice = decimal.MinValue, decimal maxPrice = decimal.MaxValue)
        {
            var products = await pdb.Products.AsNoTracking().Where(product => product.Price >= minPrice && product.Price < maxPrice).ToListAsync();


            return products;
        }

        public async Task storeProduct(Product product, Category cat, Color color, Description desc, Image image, Mass mass, MiscDetail detail, Size size)
        {
            cat.Products.Add(product);
            color.Products.Add(product);
            image.Products.Add(product);
            mass.Products.Add(product);
            detail.Products.Add(product);
            size.Products.Add(product);
            desc.Products.Add(product); 

             await pdb.AddAsync(cat);
            await pdb.AddAsync(color);
            await pdb.AddAsync(image);
            await pdb.AddAsync(mass);
            await pdb.AddAsync(detail);
            await pdb.AddAsync(size);
            await pdb.AddAsync(desc);


            

            await pdb.AddAsync(product);
            await pdb.SaveChangesAsync();
        }
    }
}
