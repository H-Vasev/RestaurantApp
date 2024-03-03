using RestaurantApp.Core.Models.Event;


namespace RestaurantApp.Core.Contracts
{
	public interface IEventService
	{
		Task<IEnumerable<EventViewModel>> GetAllEventsAsync();

        Task<EventViewModel?> GetEventByIdAsync(int id);

		Task<IEnumerable<int>?> GetAllBoockedEventIdsAsync(string userId);

		Task AddEventAsync(EventFormModel model);

		Task RemoveEventAsync(int id);

		Task<EventFormModel?> GetEventByIdForEditAsync(int id);

		Task EditEventAsync(EventFormModel model, int id);
	}
}
