using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.CapacitySlot;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
	public class CapacitySlotService : ICapacitySlotService
	{
		private readonly ApplicationDbContext dbContext;

		public CapacitySlotService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<CapacityDto> CheckCapacityAsync(string date, int peopleCount)
		{
			var capacityModel = new CapacityDto();

			var capacitySlot = await dbContext.CapacitySlots
				.Where(c => c.SlotDate.Date == DateTime.Parse(date).Date)
				.FirstOrDefaultAsync();

			if (capacitySlot == null)
			{
				capacitySlot = new CapacitySlot()
				{
					SlotDate = DateTime.Parse(date),
					CurrentCapacity = 100,
				};

				await dbContext.CapacitySlots.AddAsync(capacitySlot);
			}

			capacityModel.CapacityId = capacitySlot.Id;

			if (capacitySlot.CurrentCapacity >= peopleCount)
			{
				capacitySlot.CurrentCapacity -= peopleCount;
				await dbContext.SaveChangesAsync();

				capacityModel.IsSuccess = true;
			}
			else
			{
				capacityModel.IsSuccess = false;
				capacityModel.PeopleCountLeft = capacitySlot.CurrentCapacity;
			}

			return capacityModel;
		}
	}
}
