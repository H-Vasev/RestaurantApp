using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class CapacitySlotEntityConfiguration : IEntityTypeConfiguration<CapacitySlot>
	{
		public void Configure(EntityTypeBuilder<CapacitySlot> builder)
		{
			builder.HasData(GenerateCapacitySlots());
		}

		private static ICollection<CapacitySlot> GenerateCapacitySlots()
		{
			var capacitySlots = new List<CapacitySlot>()
			{
				new CapacitySlot()
				{
					Id = 1,
					SlotDate = DateTime.Parse(DateTime.UtcNow.AddDays(10).ToString("g")),
					TotalCapacity = 100,
					CurrentCapacity = 94
				},
				new CapacitySlot()
				{
					Id = 2,
					SlotDate = DateTime.Parse(DateTime.UtcNow.AddDays(20).ToString("g")),
					TotalCapacity = 100,
					CurrentCapacity = 94
				},
				new CapacitySlot()
				{
					Id = 3,
					SlotDate = DateTime.Parse(DateTime.UtcNow.AddDays(30).ToString("g")),
					TotalCapacity = 100,
					CurrentCapacity = 94
				},
				new CapacitySlot()
				{
					Id = 4,
					SlotDate = DateTime.Parse(DateTime.UtcNow.AddDays(45).ToString("g")),
					TotalCapacity = 100,
					CurrentCapacity = 94
				}
			};

			return capacitySlots;
		}
	}
}
