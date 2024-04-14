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

			builder.Entity<Order>()
				.Property(p => p.TotalPrice)
				.HasPrecision(18, 2);

			builder.Entity<OrderItem>()
				.Property(p => p.Price)
				.HasPrecision(18, 2);

			builder.Entity<OrderItem>()
				.HasOne(p => p.Product)
				.WithMany(p => p.OrderItems)
				.HasForeignKey(p => p.ProductId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.Entity<CartProduct>()
				.HasKey(pk => new { pk.ShoppingCartId, pk.ProductId });

			builder.Entity<Chat>()
				.HasOne(p => p.ChatUser)
				.WithMany()
				.HasForeignKey(p => p.ChatUserId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.Entity<Reservation>()
				.HasOne(p => p.CapacitySlot)
				.WithMany(p => p.Reservations)
				.HasForeignKey(p => p.CapacitySlotId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.Entity<Reservation>()
                .HasOne(p => p.ApplicationUser)
				.WithMany()
				.HasForeignKey(p => p.ApplicationUserId)
				.OnDelete(DeleteBehavior.SetNull);
				
			base.OnModelCreating(builder);
		}

        public DbSet<Town> Towns { get; set; } = null!;

		public DbSet<Address> Addresses { get; set; } = null!;

		public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;

		public DbSet<Product> Products { get; set; } = null!;	

		public DbSet<CartProduct> CartProducts { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Reservation> Reservations { get; set; } = null!;

        public DbSet<Event> Events { get; set; } = null!;

		public DbSet<GalleryImage> GalleryImages { get; set; } = null!;

		public DbSet<Order> Orders { get; set; } = null!;

		public DbSet<OrderItem> OrderItems { get; set; } = null!;

		public DbSet<Chat> Chats { get; set; } = null!;

		public DbSet<ChatMessage> ChatMessages { get; set; } = null!;

		public DbSet<CapacitySlot> CapacitySlots { get; set; } = null!;

	}
}
