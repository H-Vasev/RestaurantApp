using RestaurantApp.Core.Models.CapacitySlot;

namespace RestaurantApp.Core.Contracts
{
	public interface ICapacitySlotService
	{
		Task<CapacityDto> CheckCapacityAsync(string date, int peopleCount);
	}
}
