using System;
using System.Collections.Generic;

namespace LTWeb_DinhNgocNang_2280602045.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public decimal TotalPrice { get; set; } // Thay TotalAmount bằng TotalPrice
        public string Status { get; set; } = "Pending";
        public DateTime OrderDate { get; set; } = DateTime.Now; // Sử dụng DateTime.Now thay vì UtcNow

        public string? ShippingAddress { get; set; } // Cho phép NULL
        public string? Note { get; set; } // Cho phép NULL

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ApplicationUser User { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}