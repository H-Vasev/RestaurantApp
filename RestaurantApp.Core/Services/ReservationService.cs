using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Reservation;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
    public class ReservationService : IReservationService
	{
		private readonly ApplicationDbContext dbContext;

		public ReservationService(ApplicationDbContext data)
		{
			this.dbContext = data;
		}

		public async Task AddReservationAsync(ReservationFormModel model, string userId)
		{
			var reservation = new Reservation()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				PhoneNumber = model.PhoneNumber,
				Email = model.Email,
				Date = DateTime.Parse(model.Date),
				PeopleCount = model.PeopleCount,
				Description = model.Description,
				ApplicationUserId = Guid.Parse(userId),
				EventId = model.EventId
			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();
		}

        public async Task<IEnumerable<ReservationViewModel>> GetAllReservationAsync(string userId)
        {
			return await dbContext.Reservations
				.Where(u => u.ApplicationUserId == Guid.Parse(userId))
				.Select(r => new ReservationViewModel()
				{
					Id = r.Id.ToString(),
					FirstName = r.FirstName,
					LastName = r.LastName,
					PhoneNumber = r.PhoneNumber,
					Email = r.Email,
					Description = r.Description,
					EventName = r.Event.Title,
					PeopleCount = r.PeopleCount,
					EventId = r.EventId,
					Date = r.Date.ToString("g")
				}).ToArrayAsync();
        }
    }
}
