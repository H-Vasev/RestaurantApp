using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Models.Gallery
{
    public class GalleryViewModel
    {
        public int Id { get; set; }

        public string? Caption { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? CreatedBy { get; set; }

        public string ImagePath { get; set; } = null!;

        public string? ApplicationUserId { get; set; }
    }
}
