using RestaurantApp.Core.Models.Menu;

namespace RestaurantApp.Core.Contracts
{
    public interface IProductService
    {
        Task AddProductAsync(ProductFormModel model);
        Task EditProductAsync(ProductFormModel model, int id);
        Task<ProductFormModel> GetProductByIdForEditAsync(int id);
		Task<string> GetProductImagePathAsync(int id);
		Task RemoveProductAsync(int id);
    }
}
