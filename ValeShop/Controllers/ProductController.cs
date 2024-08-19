using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using ValeShop.Interface;
using ValeShop.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ValeShop.Data;

namespace ValeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;
        private readonly Cloudinary _cloudinary;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRepository productRepository, Cloudinary cloudinary, ApplicationDbContext context)
        {
            _repo = productRepository;
            _cloudinary = cloudinary;
            _context = context;
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

        [HttpGet]
        public IActionResult SingleProduct(Guid Id)
        {
            var singleProduct = _context.Products.FirstOrDefault(x => x.Id == Id);
            if (singleProduct == null)
            {
                return RedirectToAction("Shop", "Shop");
            }
            var image = singleProduct.ImagePath;
            ViewBag.ImageUrl = image;
            return View(singleProduct);
        }
    }
}
