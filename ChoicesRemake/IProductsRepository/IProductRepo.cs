
using ProductsModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IProductsRepository
{
    public interface IProductRepo
    {
        Task storeProduct(Product product, Category cat, Color color, Description desc, Image image, Mass mass, MiscDetail detail, Size size);
#nullable enable
        Task<Product?> getProductById(int id);
#nullable disable        
        List<Product> searchAndGetProductsByName(string name);
        Task<List<Product>> searchAndGetProductsByPriceRange(decimal minPrice = decimal.MinValue, decimal maxPrice = decimal.MaxValue);

    }
}
