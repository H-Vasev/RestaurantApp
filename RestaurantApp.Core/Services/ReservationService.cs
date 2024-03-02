﻿using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Reservation;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
	public class ReservationService : IReservationService
	{
		private readonly ApplicationDbContext dbContext;

		public ReservationService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
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

		public async Task EditReservationAsync(ReservationFormModel model, string userId, string id)
		{
			var reservation = await dbContext.Reservations
				.FirstOrDefaultAsync(r => r.ApplicationUserId == Guid.Parse(userId) && r.Id == Guid.Parse(id));

			if (reservation == null)
			{
				throw new ArgumentNullException(nameof(reservation));
			}

			reservation.FirstName = model.FirstName;
			reservation.LastName = model.LastName;
			reservation.PhoneNumber = model.PhoneNumber;
			reservation.Email = model.Email;
			reservation.Date = DateTime.Parse(model.Date);
			reservation.PeopleCount = model.PeopleCount;
			reservation.Description = model.Description;

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

        public async Task<ReservationFormModel> GetReservationByIdAsync(string userId, string id)
        {
           var reservation = await dbContext.Reservations
				.Where(r => r.ApplicationUserId == Guid.Parse(userId) && r.Id == Guid.Parse(id))
				.Select(r => new ReservationFormModel()
				{
					FirstName = r.FirstName,
					LastName = r.LastName,
					PhoneNumber = r.PhoneNumber,
					Email = r.Email,
					Description = r.Description,
					EventName = r.Event.Title,
					PeopleCount = r.PeopleCount,
					Date = r.Date.ToString("g"),
				})
				.FirstOrDefaultAsync();

			if (reservation == null)
			{
				throw new ArgumentNullException(nameof(reservation));
			}

			return reservation;
        }

        public async Task<bool> IsReservedAsync(DateTime date, string userId)
		{
			return await dbContext.Reservations
				.AnyAsync(r => r.Date.Date == date.Date && r.ApplicationUserId == Guid.Parse(userId));
		}

		public async Task RemoveReservationAsync(string userId, string id)
        {
			var reservationToRemove = await dbContext.Reservations
				.FirstOrDefaultAsync(r => r.ApplicationUserId == Guid.Parse(userId) && r.Id == Guid.Parse(id));

			if (reservationToRemove == null)
			{
				throw new ArgumentNullException(nameof(reservationToRemove));
			}

			dbContext.Reservations.Remove(reservationToRemove);
			await dbContext.SaveChangesAsync();
        }
    }
}
