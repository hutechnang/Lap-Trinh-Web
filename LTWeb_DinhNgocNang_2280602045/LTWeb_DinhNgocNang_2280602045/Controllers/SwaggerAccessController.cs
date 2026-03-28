using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTWeb_DinhNgocNang_2280602045.Controllers
{
    [AllowAnonymous]
    [Route("swagger-access")]
    public class SwaggerAccessController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/swagger/index.html");
        }
    }
}
