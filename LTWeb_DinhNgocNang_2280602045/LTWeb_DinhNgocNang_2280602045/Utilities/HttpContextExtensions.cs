using LTWeb_DinhNgocNang_2280602045.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LTWeb_DinhNgocNang_2280602045.Utilities
{
    public static class HttpContextExtensions
    {
        public static async Task<string> GetProductImageUrlAsync(this HttpContext context, int productId)
        {
            var repository = context.RequestServices.GetService<IProductRepository>();
            if (repository == null)
            {
                return "/images/default.jpg"; // URL mặc định nếu repository không có
            }

            var product = await repository.GetByIdAsync(productId);
            return product?.ImageUrl ?? "/images/default.jpg"; // Trả về URL hoặc ảnh mặc định
        }
    }
}