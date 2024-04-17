using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Event;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
	public class EventService : IEventService
	{
		private readonly ApplicationDbContext dbContext;
		private IMemoryCache memoryCache;

		public EventService(ApplicationDbContext dbContext, IMemoryCache memoryCache)
		{
			this.dbContext = dbContext;
			this.memoryCache = memoryCache;
		}

		public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync()
		{
			var cacheKey = "events";
			if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<EventViewModel> cachedEvents))
			{
				cachedEvents = await dbContext.Events
					.AsNoTracking()
					.Where(e => e.StartEvent.Date >= DateTime.Now.Date)
					.OrderBy(e => e.StartEvent)
					.Select(e => new EventViewModel
					{
						Id = e.Id,
						Title = e.Title,
						Description = e.Description,
						StartEvent = e.StartEvent
					})
					.ToArrayAsync();

				var cacheEntryOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

				memoryCache.Set(cacheKey, cachedEvents, cacheEntryOptions);
			}

			return cachedEvents;
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

		public async Task<IEnumerable<int>?> GetAllBoockedEventIdsAsync(string userId)
		{
			 var evIds = await dbContext.Events
				.Where(e => e.Reservations.Any(a => a.ApplicationUserId == Guid.Parse(userId)))
				.Select(e => e.Id)
				.ToArrayAsync();

			return evIds;
		}

		public async Task AddEventAsync(EventFormModel model)
		{
			if (model.EndEvent.Date < model.StartEvent.Date)
			{
				throw new ArgumentException("Start date must be bigger than end date!");
			}

			var ev = new Event()
			{
				Title = model.Title,
				Description = model.Description,
				StartEvent = model.StartEvent,
				EndEvent = model.EndEvent
			};

			await dbContext.Events.AddAsync(ev);
			await dbContext.SaveChangesAsync();
		}

		public async Task RemoveEventAsync(int id)
		{
			var ev = await dbContext.Events
				.FindAsync(id);

            if (ev == null)
            {
                throw new ArgumentNullException(nameof(ev));
            }

			dbContext.Events.Remove(ev);
			await dbContext.SaveChangesAsync();
        }

		public Task<EventFormModel?> GetEventByIdForEditAsync(int id)
		{
			var ev = dbContext.Events
				.Where(e => e.Id == id)
				.Select(e => new EventFormModel
				{
					Title = e.Title,
					Description = e.Description,
					StartEvent = e.StartEvent,
					EndEvent = e.EndEvent
				})
				.FirstOrDefaultAsync();

			if (ev == null)
			{
				throw new ArgumentNullException(nameof(ev));
			}

			return ev;
		}

		public async Task EditEventAsync(EventFormModel model, int id)
		{
			var ev = await dbContext.Events
				.FindAsync(id);

			if (ev == null)
			{
				throw new ArgumentNullException(nameof(ev));
			}

			ev.Title = model.Title;
			ev.Description = model.Description;
			ev.StartEvent = model.StartEvent;
			ev.EndEvent = model.EndEvent;

			await dbContext.SaveChangesAsync();
		}
	}
}
