using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class ReservationEntityConfiguration : IEntityTypeConfiguration<Reservation>
	{
		public void Configure(EntityTypeBuilder<Reservation> builder)
		{
			builder.HasData(GereateReservation());
		}

		private static ICollection<Reservation> GereateReservation()
		{
			var reservations = new List<Reservation>()
			{
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(10).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
					FirstName = "John",
					LastName = "Walker",
					Email = "john@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 1,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(10).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
					FirstName = "Jack",
					LastName = "Jackson",
					Email = "jack@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 1,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date =  DateTime.Parse(DateTime.UtcNow.AddDays(10).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
					FirstName = "Paul",
					LastName = "Robinson",
					Email = "paul@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 1,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date =  DateTime.Parse(DateTime.UtcNow.AddDays(20).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
					FirstName = "John",
					LastName = "Walker",
					Email = "john@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 2,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(20).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
					FirstName = "Jack",
					LastName = "Jackson",
					Email = "jack@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 2,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(20).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
					FirstName = "Paul",
					LastName = "Robinson",
					Email = "paul@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 2,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date =DateTime.Parse(DateTime.UtcNow.AddDays(30).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
					FirstName = "John",
					LastName = "Walker",
					Email = "john@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 3,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(30).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
					FirstName = "Jack",
					LastName = "Jackson",
					Email = "jack@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 3,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(30).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
					FirstName = "Paul",
					LastName = "Robinson",
					Email = "paul@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 3,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(45).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
					FirstName = "John",
					LastName = "Walker",
					Email = "john@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 4,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date =DateTime.Parse(DateTime.UtcNow.AddDays(45).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
					FirstName = "Jack",
					LastName = "Jackson",
					Email = "jack@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 4,
				},
				new Reservation()
				{
					Id = Guid.NewGuid(),
					Date = DateTime.Parse(DateTime.UtcNow.AddDays(45).ToString("g")),
					PeopleCount = 2,
					ApplicationUserId = Guid.Parse("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
					FirstName = "Paul",
					LastName = "Robinson",
					Email = "paul@gmail.com",
					PhoneNumber = "07708064509",
					CapacitySlotId = 4,
				},


			};

			return reservations;
		}
	}
}
