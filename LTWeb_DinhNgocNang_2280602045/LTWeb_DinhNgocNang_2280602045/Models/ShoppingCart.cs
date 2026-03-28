namespace LTWeb_DinhNgocNang_2280602045.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }

        // Tính tổng giá trị của giỏ hàng
        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.Quantity * item.Price);
        }

        // Làm trống giỏ hàng
        public void ClearCart()
        {
            Items.Clear();
        }

        // Cập nhật số lượng của một sản phẩm
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    RemoveItem(productId); // Xóa nếu số lượng <= 0
                }
            }
        }

        // Lấy tổng số lượng mặt hàng trong giỏ
        public int GetTotalItems()
        {
            return Items.Sum(item => item.Quantity);
        }

        // Kiểm tra xem giỏ hàng có trống không
        public bool IsEmpty()
        {
            return Items.Count == 0;
        }
    }
}
