using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Reservation;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;
using System.Text;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Reservation;

namespace RestaurantApp.Core.Services
{
	public class ReservationService : IReservationService
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IEventService eventService;

		public ReservationService(ApplicationDbContext dbContext, IEventService eventService)
		{
			this.dbContext = dbContext;
			this.eventService = eventService;
		}

		public async Task<string> AddReservationAsync(ReservationFormModel model, string userId, int id)
		{
			if (DateTime.Parse(model.Date).Date < DateTime.Now.Date)
			{
				return "You have enter invalid date!";
			}

			if (model.PeopleCount < PeopleCountMin || model.PeopleCount > PeopleCountMax)
			{
				return $"The number of guests must be between {PeopleCountMin} and {PeopleCountMax}.";

			}

			var date = DateTime.Parse(model.Date);
			var isReserved = await IsReservedAsync(date, userId);
			if (isReserved)
			{
				return "You already have a reservation on this date. Please review your booking details!";
			}

			var ev = await eventService.GetEventByIdAsync(id);
			if (ev != null)
			{
				model.EventId = ev.Id;
				model.EventName = ev.Title;
				model.Date = ev.StartEvent.ToString("g");
			}

			var updatedReservation = await TryUpdateReservationByDateAsync(model, userId);
			if (!string.IsNullOrEmpty(updatedReservation))
			{
				return updatedReservation;
			}

			await dbContext.SaveChangesAsync();

			return string.Empty;
		}

		public async Task<string> EditReservationAsync(ReservationFormModel model, string userId, string id)
		{
			var isReserved = await IsReservedAsync(DateTime.Parse(model.Date), userId);
			if (isReserved)
			{
				return "You already have a reservation on this date. Please review your booking details!";
			}

			if (DateTime.Parse(model.Date).Date < DateTime.Now.Date)
			{
				return "You have enter invalid date!";
			}

			if (model.PeopleCount < 1 || model.PeopleCount > 60)
			{
				return "The number of guests must be between 1 and 60.";

			}

			var currentReservation = await dbContext.Reservations
				.FirstOrDefaultAsync(r => r.ApplicationUserId == Guid.Parse(userId) && r.Id == Guid.Parse(id));

			if (currentReservation == null)
			{
				throw new ArgumentNullException(nameof(currentReservation));
			}

			if (DateTime.Parse(model.Date).Date != currentReservation.Date.Date)
			{

				var updatedReservation = await TryUpdateReservationByDateAsync(model, userId);
				if (!string.IsNullOrEmpty(updatedReservation))
				{
					return updatedReservation;
				}

				var updateCapacity = await UpdateCapacityWhenAddReservationAsync(currentReservation.CapacitySlotId, currentReservation.PeopleCount, userId);
				if (!string.IsNullOrEmpty(updateCapacity))
				{
					return updateCapacity;
				}
			}
			else if (currentReservation.PeopleCount != model.PeopleCount)
			{
				var capacitySlot = await dbContext.CapacitySlots
				.FindAsync(currentReservation.CapacitySlotId);

				if (capacitySlot == null)
				{
					throw new ArgumentNullException("Capacity slot not found");
				}

				var count = Math.Abs(currentReservation.PeopleCount - model.PeopleCount);
				if (currentReservation.PeopleCount > model.PeopleCount && capacitySlot.CurrentCapacity + count <= capacitySlot.TotalCapacity)
				{
					capacitySlot.CurrentCapacity += count;
				}
				else if (currentReservation.PeopleCount < model.PeopleCount && capacitySlot.CurrentCapacity - count >= 0)
				{
					capacitySlot.CurrentCapacity -= count;
				}
				else
				{
					return $"There are {capacitySlot.CurrentCapacity + currentReservation.PeopleCount} spaces available for booking.";
				}

				currentReservation.FirstName = model.FirstName;
				currentReservation.LastName = model.LastName;
				currentReservation.PhoneNumber = model.PhoneNumber;
				currentReservation.Email = model.Email;
				currentReservation.Date = DateTime.Parse(model.Date);
				currentReservation.PeopleCount = model.PeopleCount;
				currentReservation.Description = model.Description;
				currentReservation.CapacitySlotId = currentReservation.CapacitySlotId;
			}
			else
			{
				currentReservation.FirstName = model.FirstName;
				currentReservation.LastName = model.LastName;
				currentReservation.PhoneNumber = model.PhoneNumber;
				currentReservation.Email = model.Email;
				currentReservation.Date = DateTime.Parse(model.Date);
				currentReservation.Description = model.Description;
			}

			await dbContext.SaveChangesAsync();

			return string.Empty;
		}

		public async Task<string> TryUpdateReservationByDateAsync(ReservationFormModel model, string userId)
		{
			var capacitySlot = await dbContext.CapacitySlots
					.Where(c => c.SlotDate.Date == DateTime.Parse(model.Date).Date)
					.FirstOrDefaultAsync();

			if (capacitySlot == null)
			{
				capacitySlot = new CapacitySlot()
				{
					SlotDate = DateTime.Parse(model.Date),
					CurrentCapacity = 100,
					Reservations = new List<Reservation>()
				};

				await dbContext.CapacitySlots.AddAsync(capacitySlot);
			}

			if (capacitySlot.CurrentCapacity >= model.PeopleCount)
			{
				capacitySlot.CurrentCapacity -= model.PeopleCount;
			}
			else
			{
				return $"We have {capacitySlot.CurrentCapacity} spaces left.";
			}

			capacitySlot.Reservations.Add(new Reservation()
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
				CapacitySlotId = capacitySlot.Id
			});

			return string.Empty;
		}

		public async Task<string?> UpdateCapacityWhenAddReservationAsync(int? capacitySlotId, int peopleCount, string userId)
		{
			var capacityToUpdate = await dbContext.Reservations
					.Include(c => c.CapacitySlot)
					.FirstOrDefaultAsync(c => c.ApplicationUserId == Guid.Parse(userId) && c.CapacitySlotId == capacitySlotId);

			if (capacityToUpdate != null && capacityToUpdate.CapacitySlot!.CurrentCapacity + peopleCount <= capacityToUpdate.CapacitySlot.TotalCapacity)
			{
				capacityToUpdate.CapacitySlot.CurrentCapacity += peopleCount;
			}
			else
			{
				return $"No available spaces for this date. Please choose another one.";
			}

			dbContext.Reservations.Remove(capacityToUpdate);

			return string.Empty;
		}

		public async Task<IEnumerable<ReservationViewModel>> GetAllMineReservationAsync(string userId)
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

		public async Task<ReservationFormModel> PrepareReservationFormModelAsync(int id, string email)
		{		
			var ev = await eventService.GetEventByIdAsync(id);

			var reservation = new ReservationFormModel();

			if (ev != null)
			{
				reservation.EventName = ev.Title;
				reservation.Date = ev.StartEvent.ToString("g");
			}

			reservation.Email = email;

			return reservation;
		}

		public async Task RemoveMineReservationAsync(string userId, string id)
		{
			var reservationToRemove = await dbContext.Reservations
				.Include(r => r.CapacitySlot)
				.FirstOrDefaultAsync(r => r.ApplicationUserId == Guid.Parse(userId) && r.Id == Guid.Parse(id));

			if (reservationToRemove == null)
			{
				throw new ArgumentNullException(nameof(reservationToRemove));
			}

			if (reservationToRemove.CapacitySlot!.CurrentCapacity + reservationToRemove.PeopleCount <= 100)
			{
				reservationToRemove.CapacitySlot.CurrentCapacity += reservationToRemove.PeopleCount;
			}
			else
			{
				throw new InvalidOperationException("Invalid capacity slot");
			}

			dbContext.Reservations.Remove(reservationToRemove);
			await dbContext.SaveChangesAsync();
		}

		public async Task<string[]> GetAllFullyBookedDatesInReservationAsync()
		{
			var result = await dbContext.CapacitySlots
				.Where(c => c.CurrentCapacity == 0)
				.Select(c => c.SlotDate.ToString("yyyy/MM/dd"))
				.ToArrayAsync();

			return result;
		}

		public async Task<ReservationQuaryModel?> GetAllReservationsAsync(int? pageNumber, DateTime? startDate, DateTime? endDate, string? name)
		{
			if (startDate != null && endDate != null && startDate.Value.Date > endDate.Value.Date)
			{
				throw new InvalidOperationException("Invalid date range");
			}

			var reservationModel = new ReservationQuaryModel();

			var currentPageNumber = pageNumber ?? 1;
			var recordsPerPAge = reservationModel.RecordsPerPage;

			var query = dbContext.Reservations
				.AsNoTracking()
				.Where(r => r.ApplicationUserId != null)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(name))
			{
				query = query
					.Where(r => r.FirstName.Contains(name) || r.LastName.Contains(name));
			}

			if (startDate != null || endDate != null)
			{
				query = query
					.Where(r => (startDate == null || r.Date.Date >= startDate.Value.Date))
					.Where(r => (endDate == null || r.Date.Date <= endDate.Value.Date));
			}

			query = query.OrderBy(r => r.Date);

			reservationModel.TotalPageRecords = query.Count();
			reservationModel.Name = name;
			reservationModel.StartDate = startDate;
			reservationModel.EndDate = endDate;

			var reservations = await query
				.Skip((currentPageNumber - 1) * recordsPerPAge)
				.Take(recordsPerPAge)
				.Select(r => new ReservationTableViewModel()
				{
					Id = r.Id.ToString(),
					UserId = r.ApplicationUserId.ToString()!,
					Name = $"{r.FirstName} {r.LastName}",
					PhoneNumber = r.PhoneNumber,
					Email = r.Email,
					PeopleCount = r.PeopleCount,
					Date = r.Date.ToString("g")
				}).ToArrayAsync();

			reservationModel.Reservations = reservations;

			return reservationModel;
		}

		public async Task<StringBuilder?> DownloadAllFilteredReservationsAsync(string? name, DateTime? startDate, DateTime? endDate)
		{
			if (startDate != null && endDate != null && startDate.Value.Date > endDate.Value.Date)
			{
				return null;
			}

			var query = dbContext.Reservations
				.AsNoTracking()
				.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
				query = query
					.Where(r => r.FirstName.Contains(name) || r.LastName.Contains(name));
			}

			if (startDate != null || endDate != null)
			{
				query = query
					.Where(r => (startDate == null || r.Date.Date >= startDate.Value.Date))
					.Where(r => (endDate == null || r.Date.Date <= endDate.Value.Date));
			}

			var reservations = await query
				.OrderBy(r => r.Date)
				.Select(r => new ReservationTableViewModel()
				{
					Name = $"{r.FirstName} {r.LastName}",
					PhoneNumber = r.PhoneNumber,
					Email = r.Email,
					PeopleCount = r.PeopleCount,
					Date = r.Date.ToString("g")
				}).ToArrayAsync();

			var sb = new StringBuilder();
			sb.AppendLine("Name, Date, Guests, Phone Number, Email");

			foreach (var reservation in reservations)
			{
				sb.AppendLine($"{reservation.Name}, {reservation.Date}, {reservation.PeopleCount}, \"{reservation.PhoneNumber}\", {reservation.Email}");
			}

			return sb;
		}
	}
}
