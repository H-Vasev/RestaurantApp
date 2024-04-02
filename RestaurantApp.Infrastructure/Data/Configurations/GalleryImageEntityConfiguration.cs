using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
    public class GalleryImageEntityConfiguration : IEntityTypeConfiguration<GalleryImage>
    {
        public void Configure(EntityTypeBuilder<GalleryImage> builder)
        {
            if (DatabaseSeedController.SeedEnabled)
            {
                builder.HasData(GenerateGalleryImages());
            }
        }

        private GalleryImage[] GenerateGalleryImages()
        {
            return new GalleryImage[]
            {
                new GalleryImage
                {
                    Id = 1,
                    ImagePaht = "img/gallery/gallery-1.jpg",
                    CreatedOn = DateTime.Now
                },
                new GalleryImage
                {
                   Id = 2,
                    ImagePaht = "img/gallery/gallery-2.jpg",
                    CreatedOn = DateTime.Now.AddDays(-1)
                },
                new GalleryImage
                {
                    Id = 3,
                    ImagePaht = "img/gallery/gallery-3.jpg",
                    CreatedOn = DateTime.Now.AddDays(-2)
                },
                new GalleryImage
                {
                    Id = 4,
                    ImagePaht = "img/gallery/gallery-4.jpg",
                    CreatedOn = DateTime.Now.AddDays(-3),                   
                },
                new GalleryImage
                {
                    Id = 5,
                    ImagePaht = "img/gallery/gallery-5.jpg",
                    CreatedOn = DateTime.Now.AddDays(-4)
                },
                new GalleryImage
                {
                    Id = 6,
                    ImagePaht = "img/gallery/gallery-6.jpg",
                    CreatedOn = DateTime.Now.AddDays(-5)
                },
                new GalleryImage
                {
                    Id = 7,
                    ImagePaht = "img/gallery/gallery-7.jpg",
                    CreatedOn = DateTime.Now.AddDays(-6)
                },
                new GalleryImage
                {
                    Id = 8,
                    ImagePaht = "img/gallery/gallery-8.jpg",
                    CreatedOn = DateTime.Now.AddDays(-7)
                }
            };
        }
    }
}
