namespace Basket.API.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShopingCartItem> Items { get; set; } = new List<ShopingCartItem>();
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public ShoppingCart()
        {
        }
    }
}
