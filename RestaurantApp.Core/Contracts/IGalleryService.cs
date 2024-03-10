
using RestaurantApp.Core.Models.Gallery;

namespace RestaurantApp.Core.Contracts
{
    public interface IGalleryService
    {
        Task<IEnumerable<GalleryViewModel>> GetAllGalleryImagesAsync();

        Task<int> IncrementImageViewsCountAsync(int id, bool isIdExist);
		Task IncrementLikeCountAsync(int id, string userId);
	}
}
