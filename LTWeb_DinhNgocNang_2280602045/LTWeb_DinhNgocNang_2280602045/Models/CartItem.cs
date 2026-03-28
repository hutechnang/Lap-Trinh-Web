// CartItem.cs
namespace LTWeb_DinhNgocNang_2280602045.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}