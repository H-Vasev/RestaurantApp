using Microsoft.AspNetCore.Identity;
using RestaurantApp.Infrastructure.Data.Models;
using static RestaurantApp.Core.Constants.AdministratorConstants;

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
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			Task.Run(async () =>
			{
				if (await roleManager.RoleExistsAsync(AdminRole))
				{
					return;
				}

				var role = new IdentityRole<Guid> { Name = AdminRole };
				await roleManager.CreateAsync(role);

				var shoppingCart = new ShoppingCart();

				var user = new ApplicationUser()
				{
					Id = Guid.Parse(AdministratorId),
					Email = AdminEmail,
					UserName = AdminUserName,
					FirsName = AdminFirstName,
					LastName = AdminLastName,
					AddressId = Guid.Parse("553FB0ED-0384-4AEB-F80D-08DC21D76B67"),
					ShoppingCartId = shoppingCart.Id,
					ShoppingCart = shoppingCart
				};

				await userManager.CreateAsync(user, AdminPassword);
				await userManager.AddToRoleAsync(user, role.Name);

			}).GetAwaiter()
			.GetResult();
		}
	}
}
