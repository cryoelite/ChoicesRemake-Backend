using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<List<Product>> searchAndGetProductsByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> searchAndGetProductsByPriceRange(double minPrice = double.Epsilon, double maxPrice = double.MaxValue)
        {
            throw new NotImplementedException();
        }

        public async Task storeProduct(Product product)
        {
            await pdb.AddAsync(product);
        }
    }
}
