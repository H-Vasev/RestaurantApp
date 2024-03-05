
using RestaurantApp.Core.Models.Gallery;

namespace RestaurantApp.Core.Contracts
{
    public interface IGalleryService
    {
        Task<IEnumerable<GalleryViewModel>> GetAllGalleryImagesAsync();
    }
}
