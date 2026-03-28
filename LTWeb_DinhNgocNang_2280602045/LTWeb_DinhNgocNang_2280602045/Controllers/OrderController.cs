using LTWeb_DinhNgocNang_2280602045.Models;
using LTWeb_DinhNgocNang_2280602045.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class OrderController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(IProductRepository productRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

    // Hiển thị trang xác nhận đơn hàng
    [HttpGet]
    public async Task<IActionResult> Checkout(int productId, int quantity = 1)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return NotFound();

        var model = new OrderCheckoutViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            ProductPrice = product.Price,
            ProductImage = product.ImageUrl,
            Quantity = quantity
        };
        return View(model);
    }

    // Xử lý thanh toán
    [HttpPost]
    public async Task<IActionResult> Checkout(OrderCheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Load lại thông tin sản phẩm nếu có lỗi
            var product = await _productRepository.GetByIdAsync(model.ProductId);
            if (product == null) return NotFound();
            model.ProductName = product.Name;
            model.ProductPrice = product.Price;
            model.ProductImage = product.ImageUrl;
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);

        var order = new Order
        {
            UserId = user.Id,
            OrderDate = DateTime.Now,
            Status = "Đã thanh toán",
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                }
            }
        };
        await _orderRepository.AddAsync(order);

        // Chuyển sang trang thông báo thành công
        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}