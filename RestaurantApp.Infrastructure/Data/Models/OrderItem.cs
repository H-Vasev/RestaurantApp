﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class OrderItem
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public Guid OrderId { get; set; }

		[ForeignKey(nameof(OrderId))]
		public Order Order { get; set; } = null!;

		public int? ProductId { get; set; }

		[ForeignKey(nameof(ProductId))]
		public Product? Product { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public decimal Price { get; set; }
	}
}
