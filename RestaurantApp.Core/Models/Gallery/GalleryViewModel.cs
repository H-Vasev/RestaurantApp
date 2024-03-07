namespace RestaurantApp.Core.Models.Gallery
{
    public class GalleryViewModel
    {
        public int Id { get; set; }

        public string? Caption { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? CreatedBy { get; set; }

        public string ImagePath { get; set; } = null!;

        public int ViewsCount { get; set; }

        public int LikesCount { get; set; }

        public string? ApplicationUserId { get; set; }
    }
}
