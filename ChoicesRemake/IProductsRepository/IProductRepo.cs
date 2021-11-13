using ProductsModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IProductsRepository
{
    public interface IProductRepo
    {
        Task<Product?> getProductById(int id);

        List<Product> searchAndGetProductsByName(string name);

        Task<List<Product>> searchAndGetProductsByPriceRange(decimal minPrice = decimal.MinValue, decimal maxPrice = decimal.MaxValue);

        Task storeProduct(Product product, Category cat, Color color, Description desc, Image image, Mass mass, MiscDetail detail, Size size);

#nullable enable
#nullable disable
    }
}