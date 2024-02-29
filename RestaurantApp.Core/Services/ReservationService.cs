using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Reservation;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Services
{
	public class ReservationService : IReservationService
	{
		private readonly ApplicationDbContext data;

		public ReservationService(ApplicationDbContext data)
		{
			this.data = data;
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

			await data.Reservations.AddAsync(reservation);
			await data.SaveChangesAsync();
		}
	}
}
