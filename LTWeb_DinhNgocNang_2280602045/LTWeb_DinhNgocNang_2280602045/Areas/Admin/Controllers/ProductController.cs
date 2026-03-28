using LTWeb_DinhNgocNang_2280602045.Models;
using LTWeb_DinhNgocNang_2280602045.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;

namespace LTWeb_DinhNgocNang_2280602045.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        private string RemoveDiacritics(string input)
        {
            var normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            var chars = normalized.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark);
            return new string(chars.ToArray()).Normalize(System.Text.NormalizationForm.FormC).ToLower();
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = RemoveDiacritics(searchString);
                products = products.Where(p => RemoveDiacritics(p.Name).Contains(search)).ToList();
            }

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "name_asc";
            ViewData["PriceSortParam"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["searchString"] = searchString;
            ViewData["sortOrder"] = sortOrder;

            products = sortOrder switch
            {
                "name_asc" => products.OrderBy(p => p.Name).ToList(),
                "name_desc" => products.OrderByDescending(p => p.Name).ToList(),
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                _ => products.OrderBy(p => p.Name).ToList(),
            };

            return View(products);
        }

        [Authorize(Roles = SD.Role_Admin)] // Chỉ Admin được thêm
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [Authorize(Roles = SD.Role_Admin)] // Chỉ Admin được thêm
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> additionalImages)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                product.Images = new List<ProductImage>();
                if (additionalImages != null && additionalImages.Any())
                {
                    foreach (var image in additionalImages)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var imagePath = await SaveImage(image);
                            product.Images.Add(new ProductImage { Url = imagePath, ProductId = product.Id });
                        }
                    }
                }

                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine("wwwroot/images", fileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + fileName;
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl, List<IFormFile> additionalImages, List<int> keepImages)
        {
            ModelState.Remove("ImageUrl");
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (imageUrl != null && imageUrl.Length > 0)
                {
                    existingProduct.ImageUrl = await SaveImage(imageUrl);
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                if (existingProduct.Images == null)
                {
                    existingProduct.Images = new List<ProductImage>();
                }

                if (keepImages == null)
                {
                    keepImages = new List<int>();
                }
                var imagesToRemove = existingProduct.Images
                    .Where(img => !keepImages.Contains(img.Id))
                    .ToList();
                foreach (var image in imagesToRemove)
                {
                    var imagePath = Path.Combine("wwwroot", image.Url.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    existingProduct.Images.Remove(image);
                }

                if (additionalImages != null && additionalImages.Any())
                {
                    foreach (var image in additionalImages)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var imagePath = await SaveImage(image);
                            existingProduct.Images.Add(new ProductImage { Url = imagePath, ProductId = existingProduct.Id });
                        }
                    }
                }

                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = SD.Role_Admin)] // Chỉ Admin được xóa
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = SD.Role_Admin)] // Chỉ Admin được xóa
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}