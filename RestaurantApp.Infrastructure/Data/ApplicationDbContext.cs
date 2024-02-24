using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Infrastructure.Data.Models;
using System.Reflection;

namespace RestaurantApp.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(ApplicationDbContext)) ??
								Assembly.GetExecutingAssembly();

			builder.ApplyConfigurationsFromAssembly(assembly);

			builder.Entity<ShoppingCart>()
				.Property(p => p.Price)
				.HasPrecision(18, 2);

			builder.Entity<Product>()
				.Property(p => p.Price)
				.HasPrecision(18, 2);

			builder.Entity<CartProduct>()
				.HasKey(pk => new { pk.ShoppingCartId, pk.ProductId });

			base.OnModelCreating(builder);
		}

        public DbSet<Town> Towns { get; set; } = null!;

		public DbSet<Address> Addresses { get; set; } = null!;

		public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;

		public DbSet<Product> Products { get; set; } = null!;	

		public DbSet<CartProduct> CartProducts { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;
    }
}
