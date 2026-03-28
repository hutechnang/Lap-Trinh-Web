using LTWeb_DinhNgocNang_2280602045.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LTWeb_DinhNgocNang_2280602045.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductApiController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductDto>>("https://localhost:7181/api/products");
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"https://localhost:7181/api/products/{id}");
            return View(product);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto model)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7181/api/products", model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"https://localhost:7181/api/products/{id}");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDto model)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7181/api/products/{model.Id}", model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7181/api/products/{id}");
            return RedirectToAction("Index");
        }
    }
}
    