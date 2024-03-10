using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Gallery;
using RestaurantApp.Data;

namespace RestaurantApp.Core.Services
{
	public class GalleryService : IGalleryService
    {
        private readonly ApplicationDbContext dbContext;

        public GalleryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<GalleryViewModel>> GetAllGalleryImagesAsync()
        {
            return await dbContext.GalleryImages
                .AsNoTracking()
                .Select(x => new GalleryViewModel
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    ImagePath = x.ImagePaht,
                    ViewsCount = x.ViewsCount,
                    LikesCount = x.LikesCount,
                    ApplicationUserId = x.ApplicationUserId.ToString()
                }).ToArrayAsync();
        }

        public async Task<int> IncrementImageViewsCountAsync(int id, bool isIdExist)
        {
            var image = dbContext.GalleryImages.Find(id);
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }
            if (!isIdExist)
            {
				image.ViewsCount += 1;
				await dbContext.SaveChangesAsync();
			}

            return image.ViewsCount;
        }
    }
}
