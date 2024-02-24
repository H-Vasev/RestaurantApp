using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Town;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Town
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(TownNameMaxLenght)]
		public string TownName { get; set; } = string.Empty;
	}
}
