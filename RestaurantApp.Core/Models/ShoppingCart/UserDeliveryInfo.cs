

namespace RestaurantApp.Core.Models.ShoppingCart
{
    public class UserDeliveryInfo
    {
        public string Name { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
