using ValeShop.Models;
using ValeShop.Models.Entities;

namespace ValeShop.Interface
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategoriesAsync();
        public Task<Category?> GetCategoryByIdAsync(Guid id);
        public Task<Category> CreateCategoryAsync(CategoryViewModel categoryViewModel);
        public Task<Category?> UpdateCategoryAsync(Guid id, CategoryViewModel categoryViewModel);
        public Task<Category?> DeleteCategory(Guid id);
    }
}
