using System.ComponentModel.DataAnnotations;

namespace LTWeb_DinhNgocNang_2280602045.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Range(0.01, 1000000000.00)]
        [Display(Name = "Giá sản phẩm")]
        public decimal Price { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Ảnh chính")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Hình ảnh bổ sung")]
        public List<ProductImage>? Images { get; set; }

        [Display(Name = "Loại sản phẩm")]

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}