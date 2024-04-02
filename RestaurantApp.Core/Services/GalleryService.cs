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

        public async Task<int> IncrementImageViewsCountAsync(int id, bool isSeenByUser)
        {
            var image = dbContext.GalleryImages.Find(id);
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }
            if (!isSeenByUser)
            {
				image.ViewsCount += 1;
				await dbContext.SaveChangesAsync();
			}

            return image.ViewsCount;
        }

		public async Task IncrementLikeCountAsync(int id, string userId)
		{
            var image = await dbContext.GalleryImages
                .FindAsync(id);

            if (image == null)
            {
              throw new ArgumentNullException(nameof(image));
            }

            var isUserLiked = image.ApplicationUserId;
            if (isUserLiked != Guid.Parse(userId))
            {
                image.LikesCount += 1;
                image.ApplicationUserId = Guid.Parse(userId);

                await dbContext.SaveChangesAsync();
            }
        }
	}
}
