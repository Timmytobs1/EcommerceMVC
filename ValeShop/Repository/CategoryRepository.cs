using Microsoft.EntityFrameworkCore;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models;
using ValeShop.Models.Entities;

namespace ValeShop.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var category = new Category()
            {
                Name = categoryViewModel.Name,
               ParentId = categoryViewModel.ParentId,
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var category = await _context.Categories.ToListAsync();
            return category;
        }
        public Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<Category?> UpdateCategoryAsync(Guid id, CategoryViewModel categoryViewModel)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return null;
            }
            category.Name = categoryViewModel.Name;
            await _context.SaveChangesAsync();
            return category;
        }

      /*  public async Task<List<CategoryViewModel>> ViewCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return categories;

             //   return _mapper.Map<List<CategoryViewModel>>(categories);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get categories", e);
            }
        }*/
    }
}
