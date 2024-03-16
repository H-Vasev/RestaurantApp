using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Core.Constants.ModelsValidations.CheckoutFormModel;

namespace RestaurantApp.Core.Models.ShoppingCart
{
	public class CheckoutFormModel
	{
		[Required(ErrorMessage = "Name is required")]
		[StringLength(NameMaxLenght, MinimumLength = NameMinLenght)]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Address is required")]
		[StringLength(AddressMaxLenght, MinimumLength = AddressMinLenght)]
		public string Address { get; set; } = string.Empty;

		[Required(ErrorMessage = "Postal code is required")]
		[StringLength(PostalCodeMaxLenght, MinimumLength = PostalCodeMinLenght)]
		public string PostalCode { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[StringLength(EmailMaxLenght, MinimumLength = EmailMinLenght)]
		[EmailAddress]
        public string Email { get; set; } = string.Empty;

		[Phone]
        public string? PhoneNumber { get; set; }

        public decimal TotalPrice { get; set; }

		public IEnumerable<ShoppingCartViewModel> Items { get; set; } = new List<ShoppingCartViewModel>();

	}
}
