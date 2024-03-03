using RestaurantApp.Core.Models.Menu;

namespace RestaurantApp.Core.Contracts
{
	public interface IMenuService
	{
        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        Task<IEnumerable<ProductViewModel>> GetMenuAsync(string? category);

		Task<ProductViewModel> GetProductByIdAsync(int id);
	}
}
