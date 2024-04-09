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
		private readonly IEventService eventService;
		private readonly ICapacitySlotService capacitySlot;

		public ReservationService(ApplicationDbContext dbContext, IEventService eventService, ICapacitySlotService capacitySlot)
		{
			this.dbContext = dbContext;
			this.eventService = eventService;
			this.capacitySlot = capacitySlot;
		}

		public async Task<string> AddReservationAsync(ReservationFormModel model, string userId, int id)
		{
			if (DateTime.Parse(model.Date).Date < DateTime.Now.Date)
			{
				return "You have enter invalid date!";
			}

			if (model.PeopleCount < 1 || model.PeopleCount > 60)
			{
				return "The number of persons must be between 1 and 60";

			}

			var ev = await eventService.GetEventByIdAsync(id);

			if (ev != null)
			{
				model.EventId = ev.Id;
				model.EventName = ev.Title;
				model.Date = ev.StartEvent.ToString("g");
			}
			var date = DateTime.Parse(model.Date);
			var isReserved = await IsReservedAsync(date, userId);

			if (isReserved)
			{
				return "You have already made a reservation for this date please check your Reservation!";
			}

			var capacityModel = await capacitySlot.CheckCapacityAsync(model.Date, model.PeopleCount);

			if (!capacityModel.IsSuccess)
			{
				return $"We have {capacityModel.PeopleCountLeft} spaces left.";
			}

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
				EventId = model.EventId,
				CapacitySlotId = capacityModel.CapacityId
			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();

			return string.Empty;
		}

		public async Task<string> EditReservationAsync(ReservationFormModel model, string userId, string id)
		{
			if (DateTime.Parse(model.Date) < DateTime.Now)
			{
				return "Date must be bigger than today!";
			}

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

			return string.Empty;
		}

		public async Task<IEnumerable<ReservationViewModel>> GetAllReservationAsync(string userId)
		{
			return await dbContext.Reservations
				.AsNoTracking()
				.OrderBy(r => r.Date)
				.Where(u => u.ApplicationUserId == Guid.Parse(userId) && u.Date.Date >= DateTime.Now.Date)
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

		public async Task<ReservationFormModel> PrepareReservationFormModelAsync(int id)
		{
			var ev = await eventService.GetEventByIdAsync(id);

			var reservation = new ReservationFormModel();

			if (ev != null)
			{
				reservation.EventName = ev.Title;
				reservation.Date = ev.StartEvent.ToString("g");
			}

			return reservation;
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
