using LTWeb_DinhNgocNang_2280602045.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace LTWeb_DinhNgocNang_2280602045.Services
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return new List<CartItem>(); // Trả về giỏ rỗng nếu không có user
            }

            return await _context.CartItems
                .Where(ci => ci.UserId == user.Id)
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public async Task AddToCartAsync(int productId, int quantity = 1)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                throw new UnauthorizedAccessException("Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == user.Id && ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                throw new UnauthorizedAccessException("Vui lòng đăng nhập để xóa sản phẩm.");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == user.Id && ci.ProductId == productId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return; // Không làm gì nếu không có user
            }

            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == user.Id)
                .ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalPriceAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return 0m; // Trả về 0 nếu không có user
            }

            return await _context.CartItems
                .Where(ci => ci.UserId == user.Id)
                .SumAsync(ci => ci.Quantity * ci.Price);
        }

        public async Task<int> GetTotalItemsAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return 0; // Trả về 0 nếu không có user
            }

            return await _context.CartItems
                .Where(ci => ci.UserId == user.Id)
                .SumAsync(ci => ci.Quantity);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        }
    }
}