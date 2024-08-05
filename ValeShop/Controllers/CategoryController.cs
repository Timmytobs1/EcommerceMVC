using Microsoft.AspNetCore.Mvc;
using ValeShop.Interface;
using ValeShop.Models;

namespace ValeShop.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository category)
        {
            _categoryRepository = category;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    
        [HttpPost]
        public async Task<IActionResult> Create( CategoryViewModel categoryViewModel)
        {
            
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var category = await _categoryRepository.CreateCategoryAsync(categoryViewModel);

                TempData["SuccessMessage"] = "Category successfully registered";
                return RedirectToAction(nameof(Success));      
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }



        [HttpPut]
        public async Task<IActionResult> UpdateCategory( CategoryViewModel categoryViewModel,  Guid id)
        {
            var category = await _categoryRepository.UpdateCategoryAsync(id, categoryViewModel);
            if (category == null)
            {
                return View("Not found");
            }
            return View(category);
        }
    }
}
