using RestaurantApp.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Data.Models
{
    public class GalleryImage
    {
        [Key]
        public int Id { get; set; }

        [StringLength(DataConstants.GalleryImage.CaptionMaxLenght)]
        public string? Caption { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? CreatedBy { get; set; }

        [Required]
        public string ImagePaht { get; set; } = null!;

        public int ViewsCount { get; set; }

        public int LikesCount { get; set; }

        public Guid? ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
