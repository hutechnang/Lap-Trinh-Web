using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LTWeb_DinhNgocNang_2280602045.Models;
using System.Linq;

namespace LTWeb_DinhNgocNang_2280602045.Controllers
{
    public class CategoryDisplayController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryDisplayController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous] // Cho phép truy cập công khai
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}