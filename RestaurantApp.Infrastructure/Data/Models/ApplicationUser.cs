using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RestaurantApp.Infrastructure.Constants.DataConstants.User;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public ApplicationUser()
		{
			this.Id = Guid.NewGuid();
		}

		[MaxLength(FirstNameMaxLenght)]
		public string FirsName { get; set; } = string.Empty;

		[MaxLength(LastNameMaxLenght)]
		public string LastName { get; set; } = string.Empty;

        public Guid AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

        public Guid ShoppingCartId { get; set; }

		[ForeignKey(nameof(ShoppingCartId))]
        public ShoppingCart ShoppingCart { get; set; } = null!;
	}
}
