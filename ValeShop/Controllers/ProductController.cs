using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using ValeShop.Interface;
using ValeShop.Models;
using System.Threading.Tasks;

namespace ValeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;
        private readonly Cloudinary _cloudinary;

        public ProductController(IProductRepository productRepository, Cloudinary cloudinary)
        {
            _repo = productRepository;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
           // if (!ModelState.IsValid)
         //   {
          //      return BadRequest(ModelState);
          //  }

            var valid = await _repo.CheckCategory(productViewModel.CategoryId);
            if (!valid)
            {
                return BadRequest("Category does not exist");
            }
            string imageUrl = null;
            if (productViewModel.ImageFile != null)
            {
                using (var stream = productViewModel.ImageFile.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(productViewModel.ImageFile.FileName, stream),
                        Folder = "product_images"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    imageUrl = uploadResult.SecureUrl.ToString();
                }
            }
            var product = await _repo.CreateProduct(productViewModel, imageUrl);
            TempData["SuccessMessage"] = "Product successfully registered";
            return RedirectToAction(nameof(Success));
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
