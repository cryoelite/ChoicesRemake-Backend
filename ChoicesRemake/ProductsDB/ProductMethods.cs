using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiffMatchPatch;
using IProductsDB;
using Microsoft.EntityFrameworkCore;
using ProductsDBLayer;
using ProductsModel;

namespace ProductsDB
{
    public class ProductMethods : ProductInterface
    {
        ProductsDbContext pdb;
        public ProductMethods(ProductsDbContext dbContext) => (pdb) = (dbContext);
        public async Task<Product?> getProductById(int id)
        {
            var product = await pdb.products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            return product;
        }

        public async Task<List<Product>> searchAndGetProductsByName(string name)
        {

            var dmp = new diff_match_patch();


            var products = await pdb.products.AsNoTracking().Where(delegate (Product product)
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

        public async Task<List<Product>> searchAndGetProductsByPriceRange(double minPrice = double.Epsilon, double maxPrice = double.MaxValue)
        {
            var products = await pdb.products.AsNoTracking().Where(product => product.price >= minPrice && product.price < maxPrice).ToListAsync();


            return products;
        }

        public async Task storeProduct(Product product)
        {
            await pdb.AddAsync(product);
            await pdb.SaveChangesAsync();
        }
    }
}
