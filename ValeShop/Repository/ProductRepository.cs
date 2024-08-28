using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models.Entities;
using ValeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ValeShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<bool> CheckCategory(Guid categoryId)
        {
            return _context.Categories.AnyAsync(x => x.Id == categoryId);
        }
        public async Task<Product> CreateProduct(ProductViewModel productViewModel, string imageUrl)
        {
            var productModel = new Product
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Price = productViewModel.Price,
                CategoryId = productViewModel.CategoryId,
                ImagePath = imageUrl
            };

            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();

            return productModel;
        }
        public Task<Product?> DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetProductById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> SearchProductsAsync(string query, Guid? categoryId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return await _context.Products
                        .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
                        .ToListAsync();
                }

                return await _context.Products
                    .Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) &&
                                (!categoryId.HasValue || p.CategoryId == categoryId.Value))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
             //   _logger.LogError(ex, "Failed to search products");
                throw new Exception("Failed to search products", ex);
            }
        }

        public Task<Product?> UpdateProduct(Guid id, ProductViewModel productViewModel)
        {
            throw new NotImplementedException();
        }
       // Other methods...
    }
}
