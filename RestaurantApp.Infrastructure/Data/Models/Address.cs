using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Address;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Address
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(StreetMaxLenght)]
		public string Street { get; set; } = string.Empty;

		[Required]
		[MaxLength(PostalCodeMaxLenght)]
		public string PostalCode { get; set; } = string.Empty;

		[Required]
        public int TownId { get; set; }

		[ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

	}
}
