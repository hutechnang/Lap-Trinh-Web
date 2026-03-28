using LTWeb_DinhNgocNang_2280602045.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTWeb_DinhNgocNang_2280602045.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")] // Đã cập nhật để bao gồm Employee
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.SortOrder = sortOrder ?? "date_desc"; // Mặc định giảm dần theo ngày

            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();

            // Sắp xếp theo OrderDate dựa trên sortOrder
            switch (sortOrder)
            {
                case "date_asc":
                    orders = orders.OrderBy(o => o.OrderDate).ToList();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.OrderDate).ToList();
                    break;
                default:
                    orders = orders.OrderByDescending(o => o.OrderDate).ToList();
                    break;
            }

            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Invoice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Chuyển đổi OrderDate sang múi giờ địa phương nếu cần
            order.OrderDate = TimeZoneInfo.ConvertTimeFromUtc(order.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

            return View("~/Views/ProductDisplay/Invoice.cshtml", order); // Đảm bảo đường dẫn view đúng
        }
    }
}