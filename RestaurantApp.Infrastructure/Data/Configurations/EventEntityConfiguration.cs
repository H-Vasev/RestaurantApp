using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class EventEntityConfiguration : IEntityTypeConfiguration<Event>
	{
		public void Configure(EntityTypeBuilder<Event> builder)
		{
			if (DatabaseSeedController.SeedEnabled)
			{
                builder.HasData(GenerateEvents());
            }
        }

		private ICollection<Event> GenerateEvents()
		{
			return new Event[]
			{
				new Event
				{
					Id = 1,
					Title = "Christmas Day",
					Description = "Christmas Day",
					StartEvent = DateTime.Parse("25.12.2024 20:00"),
					EndEvent = DateTime.Parse("25.12.2024 12:00")
				},
				new Event
				{
					Id = 2,
					Title = "Heppy New Year",
					Description = "Heppy New Year",
					StartEvent = DateTime.Parse("31.12.2024 20:00"),
					EndEvent = DateTime.Parse("31.12.2024 12:00")
				},
				new Event
				{
					Id = 3,
					Title = "Easter Sunday",
					Description = "Easter Sunday",
					StartEvent = DateTime.Parse("31.03.2024 20:00"),
					EndEvent = DateTime.Parse("31.03.2024 12:00")
				},
			};
		}
	}
}
