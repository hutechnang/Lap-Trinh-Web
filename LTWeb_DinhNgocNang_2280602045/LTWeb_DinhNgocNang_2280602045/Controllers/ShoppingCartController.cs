using LTWeb_DinhNgocNang_2280602045.Models;
using LTWeb_DinhNgocNang_2280602045.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LTWeb_DinhNgocNang_2280602045.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ShoppingCartService cartService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            Console.WriteLine($"User roles when accessing ShoppingCart Index: {string.Join(", ", roles)}");
            Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
            Console.WriteLine($"User ID: {user?.Id}");

            var cartItems = await _cartService.GetCartItemsAsync();
            ViewBag.TotalPrice = await _cartService.GetTotalPriceAsync();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            Console.WriteLine($"User roles when accessing AddToCart: {string.Join(", ", roles)}");
            Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
            Console.WriteLine($"Received product ID to add: {productId}");

            if (roles.Contains("Admin"))
            {
                return Json(new { success = false, message = "Tài khoản Admin không được phép thêm sản phẩm vào giỏ hàng." });
            }

            try
            {
                await _cartService.AddToCartAsync(productId, quantity);
                var product = await _context.Products.FindAsync(productId);
                return Json(new { success = true, message = $"Đã thêm \"{product?.Name ?? "Sản phẩm"}\" vào giỏ" });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddToCart: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm sản phẩm." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            Console.WriteLine($"User roles when accessing RemoveFromCart: {string.Join(", ", roles)}");
            Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
            Console.WriteLine($"Received product ID to remove: {productId}");

            if (roles.Contains("Admin"))
            {
                return Json(new { success = false, message = "Tài khoản Admin không được phép xóa sản phẩm khỏi giỏ hàng." });
            }

            try
            {
                await _cartService.RemoveFromCartAsync(productId);
                return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng!" });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để xóa sản phẩm." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveFromCart: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa sản phẩm." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(int? productId, int? quantity, bool isSinglePurchase = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            Console.WriteLine($"User roles when accessing Checkout (GET): {string.Join(", ", roles)}");
            Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");

            if (roles.Contains("Admin"))
            {
                TempData["Message"] = "Tài khoản Admin không được phép thanh toán.";
                return RedirectToAction("Index", "ProductDisplay");
            }

            List<OrderCheckoutViewModel> checkoutViewModel;
            if (isSinglePurchase && productId.HasValue && quantity.HasValue)
            {
                // Mua hàng đơn lẻ
                var product = await _context.Products.FindAsync(productId.Value);
                if (product == null)
                {
                    return NotFound();
                }
                checkoutViewModel = new List<OrderCheckoutViewModel>
                {
                    new OrderCheckoutViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        ProductImage = product.ImageUrl ?? string.Empty,
                        Quantity = quantity.Value,
                        ShippingAddress = string.Empty,
                        Note = string.Empty
                    }
                };
                ViewBag.IsSinglePurchase = true;
            }
            else
            {
                // Mua từ giỏ hàng
                var cartItems = await _cartService.GetCartItemsAsync();
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Message"] = "Giỏ hàng của bạn đang trống.";
                    return RedirectToAction("Index", "ProductDisplay");
                }
                checkoutViewModel = cartItems.Select(item => new OrderCheckoutViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    ProductPrice = item.Price,
                    ProductImage = item.Product?.ImageUrl ?? string.Empty,
                    Quantity = item.Quantity,
                    ShippingAddress = string.Empty,
                    Note = string.Empty
                }).ToList();
                ViewBag.IsSinglePurchase = false;
            }

            return View(checkoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutConfirmed(List<OrderCheckoutViewModel> model, string shippingAddress, string note)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            Console.WriteLine($"User roles when accessing CheckoutConfirmed (POST): {string.Join(", ", roles)}");
            Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");

            if (roles.Contains("Admin"))
            {
                TempData["Message"] = "Tài khoản Admin không được phép thực hiện thanh toán.";
                return RedirectToAction("Index", "ProductDisplay");
            }

            if (model == null || !model.Any())
            {
                TempData["Message"] = "Không có sản phẩm nào để thanh toán.";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalPrice = model.Sum(x => x.ProductPrice * x.Quantity),
                Status = "Đã thanh toán",
                ShippingAddress = shippingAddress,
                Note = note
            };

            var orderDetails = model.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.ProductPrice
            }).ToList();

            order.OrderDetails = orderDetails;
            _context.Orders.Add(order);

            bool isSinglePurchase = ViewBag.IsSinglePurchase != null && (bool)ViewBag.IsSinglePurchase;
            if (!isSinglePurchase)
            {
                await _cartService.ClearCartAsync();
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanh toán thành công! Đơn hàng đã được tạo.";
            return RedirectToAction("Success", "Order"); // Sửa lại, không dùng area=Admin
        }
    }
}