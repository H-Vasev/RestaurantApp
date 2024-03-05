using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Data.Models
{
    public class ImageStatistic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GalleryImageId { get; set; }

        [ForeignKey(nameof(GalleryImageId))]
        public GalleryImage GalleryImage { get; set; } = null!;

        public int Views { get; set; }

        public int Likes { get; set; }
    }
}
