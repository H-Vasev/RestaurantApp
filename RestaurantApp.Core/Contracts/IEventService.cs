using RestaurantApp.Core.Models.Event;

namespace RestaurantApp.Core.Contracts
{
	public interface IEventService
	{
		Task<IEnumerable<EventViewModel>> GetAllEventsAsync();

        Task<EventViewModel?> GetEventByIdAsync(int id);
    }
}
