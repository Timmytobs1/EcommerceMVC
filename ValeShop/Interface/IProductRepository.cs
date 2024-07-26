using ValeShop.Models;
using ValeShop.Models.Entities;

namespace ValeShop.Interface
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product?> GetProductById(Guid id);
        public Task<Product> CreateProduct(ProductViewModel productViewModel, string imageUrl);
        public Task<Product?> UpdateProduct(Guid id, ProductViewModel productViewModel);
        public Task<Product?> DeleteProduct(Guid id);
        public Task<bool> CheckCategory(Guid categoryId);
    }
}
