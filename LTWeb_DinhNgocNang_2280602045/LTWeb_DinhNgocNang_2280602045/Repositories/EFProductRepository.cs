using LTWeb_DinhNgocNang_2280602045.Models;
using Microsoft.EntityFrameworkCore;

namespace LTWeb_DinhNgocNang_2280602045.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Include thông tin về category
                .Include(p => p.Images)   // Include danh sách hình ảnh
                .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            if (product.Images != null)
            {
                _context.ProductImages.AddRange(product.Images);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;

                // Xóa các ảnh không được chọn
                var imagesToRemove = existingProduct.Images
                    .Where(img => product.Images == null || !product.Images.Any(pi => pi.Id == img.Id))
                    .ToList();
                _context.ProductImages.RemoveRange(imagesToRemove);

                // Xóa các ảnh khỏi existingProduct.Images
                foreach (var image in imagesToRemove)
                {
                    existingProduct.Images.Remove(image);
                }

                // Tạo danh sách tạm thời để tránh lỗi "Collection was modified"
                var newImages = product.Images?.Where(img => img.Id == 0).ToList() ?? new List<ProductImage>();

                // Thêm các ảnh mới
                foreach (var image in newImages)
                {
                    existingProduct.Images.Add(image);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images) // Lấy danh sách hình ảnh để xóa
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                _context.ProductImages.RemoveRange(product.Images); // Xóa tất cả hình ảnh liên quan
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}