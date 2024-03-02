using Microsoft.AspNetCore.Identity;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Extensions
{
	public static class ApplicationBuilderExtentions
	{
		public static IApplicationBuilder PrepareData(this IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.CreateScope();
			var serviceProvider = serviceScope.ServiceProvider;

			SeedAdministrator(serviceProvider);

			return app;
		}

		private static void SeedAdministrator(IServiceProvider serviceProvider)
		{
			string adminRole = "Administrator";
			string adminPassword = "admin123";

			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			Task.Run(async () =>
			{
				if (await roleManager.RoleExistsAsync(adminRole))
				{
					return;
				}

				var role = new IdentityRole<Guid> { Name = adminRole };
				await roleManager.CreateAsync(role);

				var shoppingCart = new ShoppingCart();

				var user = new ApplicationUser()
				{
					Email = "admin@gmail.com",
					UserName = "Administrator",
					FirsName = "Test",
					LastName = "Test",
					AddressId = Guid.Parse("553FB0ED-0384-4AEB-F80D-08DC21D76B67"),
					ShoppingCartId = shoppingCart.Id,
					ShoppingCart = shoppingCart
				};

				await userManager.CreateAsync(user, adminPassword);
				await userManager.AddToRoleAsync(user, role.Name);

			}).GetAwaiter()
			.GetResult();
		}
	}
}
