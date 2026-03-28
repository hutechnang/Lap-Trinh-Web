// OrderCheckoutViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace LTWeb_DinhNgocNang_2280602045.Models
{
    public class OrderCheckoutViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; } = 1;
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
        public string ShippingAddress { get; set; }
        public string Note { get; set; }
    }
}