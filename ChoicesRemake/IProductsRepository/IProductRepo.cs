using ProductsModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IProductsRepository
{
    public interface IProductRepo
    {
        Task<Product?> getProductById(int id);

        Task<List<Product>?> searchAndGetProductsByName(string name);

        Task<List<Product>?> searchAndGetProductsByPriceRange(decimal minPrice = decimal.MinValue, decimal maxPrice = decimal.MaxValue);

        Task storeProduct(Product product);

        protected Task<List<Category>?> getCategories(Product product);

        protected Task<List<Color>?> getColors(Product product);

        protected Task<List<Description>?> getDescriptions(Product product);

        protected Task<List<MiscDetail>?> getDetails(Product product);

        protected Task<List<Image>?> getImages(Product product);

        protected Task<List<Mass>?> getMasses(Product product);

        protected Task<List<Size>?> getSizes(Product product);
    }
}