using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.HasData(GenerateUsers());
		}

		public static ICollection<ApplicationUser> GenerateUsers()
		{
			var passwordHasher = new PasswordHasher<ApplicationUser>();

			var users = new List<ApplicationUser>
			{
				new ApplicationUser
				{
					Id = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
					UserName = "john",
					NormalizedUserName = "JOHN",
					Email = "john@gmail.com",
					NormalizedEmail = "JOHN@GMAIL.COM",
					FirsName = "John",
					LastName = "Walker",
					AddressId = Guid.Parse("553FB0ED-0384-4AEB-F80D-08DC21D76B67"),
					ShoppingCartId = Guid.Parse("eaeca7c9-c81f-4d4a-aed6-8eae37e1460f"),
					SecurityStamp = Guid.NewGuid().ToString(),
					EmailConfirmed = true,
					PasswordHash = passwordHasher.HashPassword(null, "john123")
				},
				new ApplicationUser
				{
					Id = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
					UserName = "jack",
					NormalizedUserName = "JACK",
					Email = "jack@gmail.com",
					NormalizedEmail = "JACK@GMAIL.COM",
					FirsName = "Jack",
					LastName = "Jackson",
					AddressId = Guid.Parse("553FB0ED-0384-4AEB-F80D-08DC21D76B67"),
					ShoppingCartId = Guid.Parse("b574b35d-9fcb-4403-aca3-0feb4479804b"),
					SecurityStamp = Guid.NewGuid().ToString(),
					EmailConfirmed = true,
					PasswordHash = passwordHasher.HashPassword(null, "jack123")
				},
				new ApplicationUser
				{
					Id = Guid.Parse("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
					UserName = "paul",
					NormalizedUserName = "PAUL",
					Email = "paul@gmail.com",
					NormalizedEmail = "PAUL@GMAIL.COM",
					FirsName = "Paul",
					LastName = "Robinson",
					AddressId = Guid.Parse("553FB0ED-0384-4AEB-F80D-08DC21D76B67"),
					ShoppingCartId = Guid.Parse("9e3cdb70-6f53-4a89-ae00-639f5e265219"),
					SecurityStamp = Guid.NewGuid().ToString(),
					EmailConfirmed = true,
					PasswordHash = passwordHasher.HashPassword(null, "paul123")
				},
			};

			return users;
		}
	}
}
