using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Event;
using RestaurantApp.Data;

namespace RestaurantApp.Core.Services
{
	public class EventService : IEventService
	{
		private readonly ApplicationDbContext dbContext;

		public EventService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync()
		{
			return await dbContext.Events
				.AsNoTracking()
				.OrderBy(e => e.StartEvent)
				.Select(e => new EventViewModel
				{
					Id = e.Id,
					Title = e.Title,
					Description = e.Description,
					StartEvent = e.StartEvent
				})
				.ToArrayAsync();
		}

        public async Task<EventViewModel?> GetEventByIdAsync(int id)
        {
           var ev = await dbContext.Events
				.AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EventViewModel
				{
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    StartEvent = e.StartEvent
                })
                .FirstOrDefaultAsync();

			return ev;
        }
    }
}
