using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LTWeb_DinhNgocNang_2280602045.Models;
using LTWeb_DinhNgocNang_2280602045.Models.ViewModels;

namespace LTWeb_DinhNgocNang_2280602045.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ Admin được tạo Employee
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View(new CreateEmployeeViewModel());

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = "" // Gán giá trị mặc định cho PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    TempData["SuccessMessage"] = "Tạo tài khoản nhân viên thành công."; // Thêm thông báo
                    return RedirectToAction("Index", "Product", new { area = "Admin" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("CreateEmployee", model); // Gọi rõ tên view
        }
    }
}