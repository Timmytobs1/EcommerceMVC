using Microsoft.AspNetCore.Mvc;
using ValeShop.Interface;

namespace ValeShop.Controllers
{
  
        public class SearchController : Controller
        {
            private readonly IProductRepository _productRepository;
            private readonly ICategoryRepository _categoryRepository;

            public SearchController(IProductRepository productRepository, ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
                _productRepository = productRepository;
            }

            [HttpGet]
            [Route("SearchResults/Results")]
            public async Task<IActionResult> Results(string query, Guid? category)
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                ViewBag.Categories = categories;
                ViewBag.SearchQuery = query;

                var results = await _productRepository.SearchProductsAsync(query, category);
                return View("SearchResults", results);
            }
        }

       
    
}
