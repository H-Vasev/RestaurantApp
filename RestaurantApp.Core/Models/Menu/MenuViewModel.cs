using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Core.Models.Menu
{
	public class MenuViewModel
	{
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? Image { get; set; }

		public decimal Price { get; set; }

		public int CategoryId { get; set; }
	}
}
