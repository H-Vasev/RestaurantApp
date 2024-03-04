using RestaurantApp.Core.Models.Menu;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Areas.Administrator.Models.Product
{
    public class ProductFormModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public string? Image { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
    }
}
