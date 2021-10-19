
using ProductsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IProductsDB
{
    public interface ProductInterface
    {
        Task storeProduct(Product product);
        Task<Product?> getProductById(int id);
        Task<List<Product>> searchAndGetProductsByName(string name);
        Task<List<Product>> searchAndGetProductsByPriceRange(double minPrice = double.Epsilon, double maxPrice = double.MaxValue);

    }
}
