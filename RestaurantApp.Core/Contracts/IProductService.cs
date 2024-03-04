using RestaurantApp.Core.Models.Menu;

namespace RestaurantApp.Core.Contracts
{
    public interface IProductService
    {
        Task AddProductAsync(ProductFormModel model);
		Task<string> GetProductImagePathAsync(int id);
		Task RemoveProductAsync(int id);
    }
}
