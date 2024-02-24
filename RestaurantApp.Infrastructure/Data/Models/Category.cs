using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Category;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Category
	{
		[Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLenght)]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
