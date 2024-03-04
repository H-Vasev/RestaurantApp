using RestaurantApp.Core.Models.Menu;

namespace RestaurantApp.Core.Contracts
{
    public interface IProductService
    {
        Task AddProductAsync(ProductFormModel model);
    }
}
