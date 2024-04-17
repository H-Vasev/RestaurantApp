using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Event;
using RestaurantApp.Core.Models.Reservation;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
	public class ReservationServiceTests
	{
		private DbContextOptions<ApplicationDbContext> dbContextOptions;
		private ApplicationDbContext dbContext;
		private IReservationService reservationService;
		private Mock<IEventService> mockEventService;

		[SetUp]
		public void Setup()
		{
			dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			dbContext = new ApplicationDbContext(dbContextOptions);

			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();

			mockEventService = new Mock<IEventService>();
			mockEventService.Setup(service => service.GetEventByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(new EventViewModel
				{
					Id = 1,
					Title = "Event 1",
					Description = "Event 1 description",
					StartEvent = DateTime.UtcNow.AddDays(5),
				});

			reservationService = new ReservationService(dbContext, mockEventService.Object);
		}

		//AddReservationAsync

		[Test]
		public async Task AddReservationAsync_ShouldAddReservation_WhenModelIsValid()
		{
			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "123456789",
				Date = DateTime.Now.AddDays(5).ToString("g"),
				PeopleCount = 4,
				Description = "Birthday party"
			};

			var result = await reservationService.AddReservationAsync(model, Guid.NewGuid().ToString(), 1);

			Assert.That(result, Is.EqualTo(string.Empty));
			Assert.That(1, Is.EqualTo(await dbContext.Reservations.CountAsync()));
		}

		[Test]
		public async Task AddReservationAsync_ShouldReturnError_WhenDateIsBeforeToday()
		{
			var pastDate = DateTime.UtcNow.AddDays(-1).ToString("g");
			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "123456789",
				Date = pastDate,
				PeopleCount = 4,
				Description = "Birthday party"
			};

			var result = await reservationService.AddReservationAsync(model, Guid.NewGuid().ToString(), 1);

			Assert.That(result, Is.Not.EqualTo(string.Empty));
			Assert.That("You have enter invalid date!", Is.EqualTo(result));
		}

		[Test]
		public async Task AddReservationAsync_ShouldReturnErrorMessage_WhenReservationAlreadyExists()
		{
			var userId = Guid.NewGuid().ToString();
			var date = DateTime.Now.AddDays(5).ToString("g");
			var reservation = new Reservation
			{
				FirstName = "Existing",
				LastName = "Existing",
				Email = "existing@example.com",
				PhoneNumber = "987654321",
				Date = DateTime.Parse(date),
				PeopleCount = 3,
				Description = "Existing reservation",
				ApplicationUserId = Guid.Parse(userId),
				CapacitySlotId = 1
			};

			dbContext.Reservations.Add(reservation);
			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "123456789",
				Date = date,
				PeopleCount = 4,
				Description = "Birthday party",
				ApplicationUserId = Guid.Parse(userId)
			};

			var result = await reservationService.AddReservationAsync(model, userId, 1);

			Assert.That("You already have a reservation on this date. Please review your booking details!", Is.EqualTo(result));
		}

		[Test]
		public async Task AddReservationAsync_ShouldAddReservationWithEventDetails_WhenEventExists()
		{
			var userId = Guid.NewGuid().ToString();
			var date = DateTime.UtcNow.AddDays(5).ToString("g");

			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "1234567890",
				Date = date,
				PeopleCount = 5,
				Description = "Test Event Reservation"
			};

			var result = await reservationService.AddReservationAsync(model, userId, 1);

			using (var assertContext = new ApplicationDbContext(dbContextOptions))
			{
				var reservation = assertContext.Reservations.FirstOrDefault(r => r.ApplicationUserId == Guid.Parse(userId));
				Assert.That(reservation, Is.Not.EqualTo(null));
				Assert.That(model.FirstName, Is.EqualTo(reservation.FirstName));
				Assert.That(1, Is.EqualTo(reservation.EventId));
			}
		}

		[Test]
		public async Task AddReservationAsync_ShouldReturnError_WhenPeopleCountIsLessThanMinimum()
		{
			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "123456789",
				Date = DateTime.Now.AddDays(5).ToString("g"),
				PeopleCount = 0,
				Description = "Birthday party"
			};

			var result = await reservationService.AddReservationAsync(model, Guid.NewGuid().ToString(), 1);

			Assert.That("The number of guests must be between 1 and 60.", Is.EqualTo(result));
		}

		[Test]
		public async Task AddReservationAsync_ShouldReturnError_WhenPeopleCountIsBiggerThanMaximum()
		{
			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "123456789",
				Date = DateTime.Now.AddDays(5).ToString("g"),
				PeopleCount = 61,
				Description = "Birthday party"
			};

			var result = await reservationService.AddReservationAsync(model, Guid.NewGuid().ToString(), 1);

			Assert.That("The number of guests must be between 1 and 60.", Is.EqualTo(result));
		}

		[Test]
		public async Task AddReservationAsync_ShouldReturnCurrentCapacityMessage_WhenIsNotEnoughSpacesLeft()
		{
			var date = DateTime.Now.AddDays(5);

			var model = new ReservationFormModel
			{
				FirstName = "FirstName",
				LastName = "LastName",
				Email = "test@example.com",
				PhoneNumber = "123456789",
				Date = date.ToString("g"),
				PeopleCount = 20,
				Description = "Birthday party"
			};

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = date,
				CurrentCapacity = 10,
				Reservations = new List<Reservation>()
			});
			dbContext.SaveChanges();

			var result = await reservationService.AddReservationAsync(model, Guid.NewGuid().ToString(), 1);

			Assert.That(result, Is.EqualTo("We have 10 spaces left."));
		}

		//TryUpdateReservationByDateAsync
		[Test]
		public async Task TryUpdateReservationByDateAsync_ShouldReturnEmptyString_WhenCapacityIsBiggerThanReservationPeopleCount()
		{
			var userId = Guid.NewGuid().ToString();

			var model = new ReservationFormModel
			{
				Date = DateTime.Now.ToString(),
				PeopleCount = 5,
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "1234567890",
				Email = "test@example.com"
			};

			dbContext.CapacitySlots.Add(new CapacitySlot
			{
				SlotDate = DateTime.Now,
				CurrentCapacity = 100,
				Reservations = new List<Reservation>()
			});
			dbContext.SaveChanges();

			var result = await reservationService.TryUpdateReservationByDateAsync(model, userId);

			var capacity = await dbContext.CapacitySlots.FirstOrDefaultAsync();

			Assert.That(result, Is.EqualTo(string.Empty));
			Assert.That(95, Is.EqualTo(capacity.CurrentCapacity));
			Assert.That(1, Is.EqualTo(capacity.Reservations.Count));
		}

		[Test]
		public async Task TryUpdateReservationByDateAsync_ShouldReturnEmptyString_WhenCapacityIsEqualThanReservationPeopleCount()
		{
			var userId = Guid.NewGuid().ToString();

			var model = new ReservationFormModel
			{
				Date = DateTime.Now.ToString(),
				PeopleCount = 100,
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "1234567890",
				Email = "test@example.com"
			};

			dbContext.CapacitySlots.Add(new CapacitySlot
			{
				SlotDate = DateTime.Now,
				CurrentCapacity = 100,
				Reservations = new List<Reservation>()
			});
			dbContext.SaveChanges();

			var result = await reservationService.TryUpdateReservationByDateAsync(model, userId);

			var capacity = await dbContext.CapacitySlots.FirstOrDefaultAsync();

			Assert.That(result, Is.EqualTo(string.Empty));
			Assert.That(0, Is.EqualTo(capacity.CurrentCapacity));
			Assert.That(1, Is.EqualTo(capacity.Reservations.Count));
		}

		[Test]
		public async Task TryUpdateReservationByDateAsync_ShouldReturnEmptyString_WhenMultipleReservationAddedWithCorrectNumberOfPeople()
		{
			var userId = Guid.NewGuid().ToString();

			var model = new ReservationFormModel
			{
				Date = DateTime.Now.Date.ToString(),
				PeopleCount = 20,
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "1234567890",
				Email = "test@example.com"
			};

			dbContext.CapacitySlots.Add(new CapacitySlot
			{
				Id = 1,
				SlotDate = DateTime.Now,
				CurrentCapacity = 20,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						FirstName = "Test",
						LastName = "User",
						PhoneNumber = "1234567890",
						Email = "test@example.com",
						ApplicationUserId = Guid.Parse(userId),
						CapacitySlotId = 1,
						PeopleCount = 20,
						Date = DateTime.Parse(model.Date)
					},
					new Reservation
					{
						FirstName = "Test",
						LastName = "User",
						PhoneNumber = "1234567890",
						Email = "test@example.com",
						ApplicationUserId = Guid.Parse(userId),
						CapacitySlotId = 1,
						PeopleCount = 60,
						Date = DateTime.Parse(model.Date)
					},
				}
			});
			dbContext.SaveChanges();

			var result = await reservationService.TryUpdateReservationByDateAsync(model, userId);

			var capacity = await dbContext.CapacitySlots.FirstOrDefaultAsync(d => d.SlotDate.Date == DateTime.Parse(model.Date));

			Assert.That(result, Is.EqualTo(string.Empty));
			Assert.That(0, Is.EqualTo(capacity.CurrentCapacity));
			Assert.That(3, Is.EqualTo(capacity.Reservations.Count));
		}

		[Test]
		public async Task TryUpdateReservationByDateAsync_ShouldReturnCurrentCapacity_ReservationPeopleCountIsBiggerThanCapacity()
		{
			var userId = Guid.NewGuid().ToString();

			var model = new ReservationFormModel
			{
				Date = DateTime.Now.ToString(),
				PeopleCount = 101,
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "1234567890",
				Email = "test@example.com"
			};

			dbContext.CapacitySlots.Add(new CapacitySlot
			{
				SlotDate = DateTime.Now,
				CurrentCapacity = 100,
				Reservations = new List<Reservation>()
			});
			dbContext.SaveChanges();

			var result = await reservationService.TryUpdateReservationByDateAsync(model, userId);

			var capacity = await dbContext.CapacitySlots.FirstOrDefaultAsync();

			Assert.That(result, Is.EqualTo("We have 100 spaces left."));
			Assert.That(100, Is.EqualTo(capacity.CurrentCapacity));
			Assert.That(0, Is.EqualTo(capacity.Reservations.Count));
		}

		//UpdateCapacityWhenAddReservationAsync
		[Test]
		public async Task UpdateCapacityWhenAddReservationAsync_ShouldIncreaseCapacity_WhenReservationIsChangeToDifferentDate()
		{
			var date = DateTime.Now;
			var userId = Guid.NewGuid().ToString();
			var capacitiId = 1;

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				Id = capacitiId,
				SlotDate = date,
				CurrentCapacity = 40,
				Reservations = new List<Reservation>()
				{
					new Reservation()
					{
						FirstName = "Test",
						LastName = "User",
						PhoneNumber = "1234567890",
						Email = "test@example.com",
						ApplicationUserId = Guid.Parse(userId),
						CapacitySlotId = 1,
						PeopleCount = 60,
						Date = date
					}
				}
			});
			await dbContext.SaveChangesAsync();

			var result = await reservationService.UpdateCapacityWhenAddReservationAsync(capacitiId, 5, userId);
			var capacity = await dbContext.CapacitySlots.FirstOrDefaultAsync();

			Assert.That(result, Is.EqualTo(string.Empty));
			Assert.That(45, Is.EqualTo(capacity.CurrentCapacity));
			Assert.That(1, Is.EqualTo(capacity.Reservations.Count));
		}

		[Test]
		public async Task UpdateCapacityWhenAddReservationAsync_ShouldReturnMessage_WhenReservationIsChangeToDifferentDateButIsNotEnoughSpaces()
		{
			var date = DateTime.Now;
			var userId = Guid.NewGuid().ToString();
			var capacitiId = 1;

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				Id = capacitiId,
				SlotDate = date,
				CurrentCapacity = 60,
				Reservations = new List<Reservation>()
				{
					new Reservation()
					{
						FirstName = "Test",
						LastName = "User",
						PhoneNumber = "1234567890",
						Email = "test@example.com",
						ApplicationUserId = Guid.Parse(userId),
						CapacitySlotId = 1,
						PeopleCount = 1,
						Date = date
					}
				}
			});
			await dbContext.SaveChangesAsync();

			var result = await reservationService.UpdateCapacityWhenAddReservationAsync(capacitiId, 41, userId);
			var capacity = await dbContext.CapacitySlots.FirstOrDefaultAsync();

			Assert.That(result, Is.EqualTo("No available spaces for this date. Please choose another one."));
			Assert.That(1, Is.EqualTo(capacity.Reservations.Count));
		}

		[Test]
		public async Task UpdateCapacityWhenAddReservationAsync_ShouldReturnMessage_WhenReservationIsNull()
		{
			var userId = Guid.NewGuid().ToString();
			var capacitiId = 1;

			var result = await reservationService.UpdateCapacityWhenAddReservationAsync(capacitiId, 41, userId);

			Assert.That(result, Is.EqualTo("No available spaces for this date. Please choose another one."));
		}

		//GetAllReservationAsync
		[Test]
		public async Task GetAllReservationAsync_ShouldReturnAllReservationsForUser()
		{
			var userId = Guid.NewGuid().ToString();
			var reservations = new List<Reservation>()
			{
				new Reservation
				{
					ApplicationUserId = Guid.Parse(userId),
					Date = DateTime.Now.AddDays(1),
					Event = new Event { Title = "Event 1" },
					FirstName = "FirstName1",
					LastName = "LastName1",
					PhoneNumber = "0987654323",
					Email = "test1@example.com",
					PeopleCount = 4,
					Description = "Description 1"
				},
				new Reservation
				{
					ApplicationUserId = Guid.Parse(userId),
					Date = DateTime.Now.AddDays(2),
					Event = new Event { Title = "Event 2" },
					 FirstName = "FirstName2",
					LastName = "LastName12",
					PhoneNumber = "0987654321",
					Email = "test2@example.com",
					PeopleCount = 2,
					Description = "Description 2"
				}
			};

			dbContext.Reservations.AddRange(reservations);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllMineReservationAsync(userId);

			Assert.That(2, Is.EqualTo(result.Count()));
			Assert.That("FirstName1", Is.EqualTo(result.First().FirstName));
			Assert.That("Event 1", Is.EqualTo(result.First().EventName));
			Assert.That("FirstName2", Is.EqualTo(result.Last().FirstName));
			Assert.That("Event 2", Is.EqualTo(result.Last().EventName));
		}

		[Test]
		public async Task GetAllReservationAsync_ShouldReturnZeroCount()
		{
			var userId = Guid.NewGuid().ToString();
			var reservations = new Reservation
			{
				ApplicationUserId = Guid.Parse(userId),
				Date = DateTime.Now.AddDays(1),
				Event = new Event { Title = "Event 1" },
				FirstName = "FirstName1",
				LastName = "LastName1",
				PhoneNumber = "0987654323",
				Email = "test1@example.com",
				PeopleCount = 4,
				Description = "Description 1"
			};

			dbContext.Reservations.Add(reservations);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllMineReservationAsync(Guid.NewGuid().ToString());

			Assert.That(0, Is.EqualTo(result.Count()));
		}

		[Test]
		public async Task GetAllReservationAsync_ShouldReturnZeroCountWhenDateIsExpire()
		{
			var userId = Guid.NewGuid().ToString();
			var reservations = new Reservation
			{
				ApplicationUserId = Guid.Parse(userId),
				Date = DateTime.Now.AddDays(-1),
				Event = new Event { Title = "Event 1" },
				FirstName = "FirstName1",
				LastName = "LastName1",
				PhoneNumber = "0987654323",
				Email = "test1@example.com",
				PeopleCount = 4,
				Description = "Description 1"
			};

			dbContext.Reservations.Add(reservations);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllMineReservationAsync(userId);

			Assert.That(0, Is.EqualTo(result.Count()));
		}

		[Test]
		public async Task GetAllReservationAsync_ShouldReturnAllReservationForUserWhenDateIsEqual()
		{
			var userId = Guid.NewGuid().ToString();
			var reservations = new Reservation
			{
				ApplicationUserId = Guid.Parse(userId),
				Date = DateTime.Now,
				Event = new Event { Title = "Event 1" },
				FirstName = "FirstName1",
				LastName = "LastName1",
				PhoneNumber = "0987654323",
				Email = "test1@example.com",
				PeopleCount = 4,
				Description = "Description 1"
			};

			dbContext.Reservations.Add(reservations);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllMineReservationAsync(userId);

			Assert.That(1, Is.EqualTo(result.Count()));
			Assert.That("FirstName1", Is.EqualTo(result.First().FirstName));
			Assert.That("Event 1", Is.EqualTo(result.First().EventName));
		}

		[Test]
		public async Task GetAllReservationAsync_ShouldReturnReservationsOrderedByDate()
		{
			var userId = Guid.NewGuid().ToString();
			var firstDate = DateTime.UtcNow.AddDays(2);
			var secondDate = DateTime.UtcNow.AddDays(1);
			var thirdDate = DateTime.UtcNow.AddDays(3);

			var reservations = new List<Reservation>()
			{
				 new Reservation
				 {
					 ApplicationUserId = Guid.Parse(userId),
					 Date = thirdDate,
					 Event = new Event { Title = "Event 3" },
					 FirstName = "Third",
					 LastName = "Reservation",
					 PhoneNumber = "0987654323",
					 Email = "third@example.com",
					 PeopleCount = 3,
					 Description = "Third reservation"
				 },
				 new Reservation
				 {
					 ApplicationUserId = Guid.Parse(userId),
					 Date = firstDate,
					 Event = new Event { Title = "Event 1" },
					 FirstName = "First",
					 LastName = "Reservation",
					 PhoneNumber = "0987654321",
					 Email = "first@example.com",
					 PeopleCount = 1,
					 Description = "First reservation"
				 },
				 new Reservation
				 {
					 ApplicationUserId = Guid.Parse(userId),
					 Date = secondDate,
					 Event = new Event { Title = "Event 2" },
					 FirstName = "Second",
					 LastName = "Reservation",
					 PhoneNumber = "0987654322",
					 Email = "second@example.com",
					 PeopleCount = 2,
					 Description = "Second reservation"
				 },
			};

			await dbContext.Reservations.AddRangeAsync(reservations);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllMineReservationAsync(userId);

			Assert.That(3, Is.EqualTo(result.Count()));
			Assert.That("Second", Is.EqualTo(result.ElementAt(0).FirstName));
			Assert.That("First", Is.EqualTo(result.ElementAt(1).FirstName));
			Assert.That("Third", Is.EqualTo(result.ElementAt(2).FirstName));
		}


		//GetReservationByIdAsync
		[Test]
		public async Task GetReservationByIdAsync_ShouldReturnCorrectReservation()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid();

			var reservation = new Reservation
			{
				Id = reservationId,
				ApplicationUserId = Guid.Parse(userId),
				Date = DateTime.UtcNow.AddDays(5),
				Event = new Event { Title = "Event 1" },
				FirstName = "FirstName",
				LastName = "LastName",
				PhoneNumber = "123456789",
				Email = "test@example.com",
				PeopleCount = 5,
				Description = "New Year party reservation"
			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetReservationByIdAsync(userId, reservationId.ToString());

			Assert.That(result, Is.Not.EqualTo(null));
			Assert.That("FirstName", Is.EqualTo(result.FirstName));
			Assert.That("LastName", Is.EqualTo(result.LastName));
			Assert.That("123456789", Is.EqualTo(result.PhoneNumber));
			Assert.That("test@example.com", Is.EqualTo(result.Email));
			Assert.That(5, Is.EqualTo(result.PeopleCount));
			Assert.That("Event 1", Is.EqualTo(result.EventName));
			Assert.That("New Year party reservation", Is.EqualTo(result.Description));
			Assert.That(reservation.Date.ToString("g"), Is.EqualTo(result.Date));
		}

		[Test]
		public async Task GetReservationByIdAsync_ShouldReturnCorrectReservationWithNoEvent()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid();

			var reservation = new Reservation
			{
				Id = reservationId,
				ApplicationUserId = Guid.Parse(userId),
				Date = DateTime.UtcNow.AddDays(5),
				FirstName = "FirstName",
				LastName = "LastName",
				PhoneNumber = "123456789",
				Email = "test@example.com",
				PeopleCount = 5,
				Description = "New Year party reservation"
			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetReservationByIdAsync(userId, reservationId.ToString());

			Assert.That(result, Is.Not.EqualTo(null));
			Assert.That("FirstName", Is.EqualTo(result.FirstName));
			Assert.That("LastName", Is.EqualTo(result.LastName));
			Assert.That("123456789", Is.EqualTo(result.PhoneNumber));
			Assert.That("test@example.com", Is.EqualTo(result.Email));
			Assert.That(5, Is.EqualTo(result.PeopleCount));
			Assert.That("New Year party reservation", Is.EqualTo(result.Description));
			Assert.That(reservation.Date.ToString("g"), Is.EqualTo(result.Date));
		}

		[Test]
		public void GetReservationByIdAsync_ShouldThrowException_WhenReservationNotFound()
		{
			var userId = Guid.NewGuid().ToString();
			var nonExistentReservationId = Guid.NewGuid().ToString();

			var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await reservationService.GetReservationByIdAsync(userId, nonExistentReservationId));
			Assert.That(ex.ParamName, Is.EqualTo("reservation"));
		}


		//IsReservationExistsAsync
		[Test]
		public async Task IsReservedAsync_ShouldReturnTrue_WhenReservationExistsForUserOnDate()
		{
			var userId = Guid.NewGuid().ToString();
			var testDate1 = DateTime.UtcNow.AddDays(5);
			var testDate2 = DateTime.UtcNow.AddDays(5);

			var reservation = new List<Reservation>()
			{
				 new Reservation()
				 {
					ApplicationUserId = Guid.Parse(userId),
					Date = testDate1,
					Description = "Test reservation",
					PeopleCount = 5,
					Email = "test@test.com",
					FirstName = "FirstName",
					LastName = "LastName",
					PhoneNumber = "123456789",
				 },
				 new Reservation()
				 {
					ApplicationUserId = Guid.Parse(userId),
					Date = testDate2,
					Description = "Test reservation",
					PeopleCount = 5,
					Email = "test@test.com",
					FirstName = "FirstName2",
					LastName = "LastName2",
					PhoneNumber = "123456784",
				 }
			};


			await dbContext.Reservations.AddRangeAsync(reservation);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.IsReservedAsync(testDate1, userId);
			var result2 = await reservationService.IsReservedAsync(testDate2, userId);

			Assert.That(result, Is.EqualTo(true));
			Assert.That(result2, Is.EqualTo(true));
		}

		[Test]
		public async Task IsReservedAsync_ShouldReturnFalse_WhenNoReservationExistsForUserOnDate()
		{
			var userId = Guid.NewGuid().ToString();
			var testDate = DateTime.UtcNow.AddDays(5);

			var reservation = new Reservation
			{
				ApplicationUserId = Guid.Parse(userId),
				Date = testDate,
				Description = "Test reservation",
				PeopleCount = 5,
				Email = "test@test.com",
				FirstName = "FirstName",
				LastName = "LastName",
				PhoneNumber = "123456789",
			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.IsReservedAsync(DateTime.Now.AddDays(1), userId);

			Assert.That(result, Is.EqualTo(false));
		}

		[Test]
		public async Task IsReservedAsync_ShouldReturnFalse_WhenNoReservationNotExistsForUser()
		{
			var userId = Guid.NewGuid().ToString();
			var testDate = DateTime.UtcNow.AddDays(5);

			var reservation = new Reservation
			{
				ApplicationUserId = Guid.Parse(userId),
				Date = testDate,
				Description = "Test reservation",
				PeopleCount = 5,
				Email = "test@test.com",
				FirstName = "FirstName",
				LastName = "LastName",
				PhoneNumber = "123456789",
			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();

			var result = await reservationService.IsReservedAsync(testDate, Guid.NewGuid().ToString());

			Assert.That(result, Is.EqualTo(false));
		}

		//PrepareReservationFormModel
		[Test]
		public async Task PrepareReservationFormModelAsync_ShouldCorrectlyMapEventDetails_WhenEventExists()
		{
			var eventId = 1;
			var reservationService = new ReservationService(null, mockEventService.Object);

			var result = await reservationService.PrepareReservationFormModelAsync(eventId, "test@gmail.com");

			Assert.That(result, Is.Not.EqualTo(null));
			Assert.That("Event 1", Is.EqualTo(result.EventName));
			Assert.That("test@gmail.com", Is.EqualTo(result.Email));
			Assert.That(DateTime.UtcNow.AddDays(5).ToString("g"), Is.EqualTo(result.Date));
		}

		[Test]
		public async Task PrepareReservationFormModelAsync_ShouldReturtModel()
		{
			var reservationService = new ReservationService(null, mockEventService.Object);

			var result = await reservationService.PrepareReservationFormModelAsync(2, "test@gmail.com");

			Assert.That(result, Is.Not.EqualTo(null));
			Assert.That(result, Is.InstanceOf<ReservationFormModel>());
		}

		//RemoveReservationAsync
		[Test]
		public async Task RemoveReservationAsync_ShouldRemoveReservation_WhenReservationExists()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid();

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 90,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = reservationId,
						ApplicationUserId = Guid.Parse(userId),
						Date = DateTime.UtcNow.AddDays(5),
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});

			await dbContext.SaveChangesAsync();


			await reservationService.RemoveMineReservationAsync(userId, reservationId.ToString());

			var removedReservation = await dbContext.Reservations
				.FirstOrDefaultAsync(r => r.Id == reservationId);

			Assert.That(removedReservation, Is.Null);

		}

		[Test]
		public async Task RemoveReservationAsync_ShouldThrowException_WhenCapacityIsFull()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid();

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 100,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = reservationId,
						ApplicationUserId = Guid.Parse(userId),
						Date = DateTime.UtcNow.AddDays(5),
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});

			await dbContext.SaveChangesAsync();

			var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
							await reservationService.RemoveMineReservationAsync(userId, reservationId.ToString()));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.Message, Is.EqualTo("Invalid capacity slot"));

		}

		[Test]
		public async Task RemoveReservationAsync_ShouldThrowException_WhenReservationIdDoNotExist()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationToRemove = Guid.NewGuid();

			var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await reservationService.RemoveMineReservationAsync(userId, reservationToRemove.ToString()));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.ParamName, Is.EqualTo("reservationToRemove"));
		}

		[Test]
		public async Task RemoveReservationAsync_ShouldThrowException_WhenReservationDoesNotBelongToUser()
		{
			var ownerUserId = Guid.NewGuid().ToString();
			var otherUserId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid();

			var reservationToRemove = new Reservation
			{
				Id = reservationId,
				ApplicationUserId = Guid.Parse(ownerUserId),
				Date = DateTime.UtcNow.AddDays(5),
				FirstName = "Test",
				LastName = "User",
				PhoneNumber = "123456789",
				Email = "test@example.com",
				PeopleCount = 4,
				Description = "Test",
				CapacitySlotId = 1
			};

			await dbContext.Reservations.AddAsync(reservationToRemove);
			await dbContext.SaveChangesAsync();

			var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await reservationService.RemoveMineReservationAsync(otherUserId, reservationId.ToString()));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.ParamName, Is.EqualTo("reservationToRemove"));
		}

		//EditReservationAsync
		[Test]
		public async Task EditReservationAsync_ShouldUpdateReservation_WhenDateAndPeopleCountNotChanget()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var date = DateTime.Now.AddDays(1);

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 95,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = date,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});

			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = date.ToString("g"),
				PeopleCount = 5,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			Assert.That(result, Is.EqualTo(string.Empty));

			var updatedReservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == Guid.Parse(reservationId));
			Assert.That(updatedReservation!.FirstName, Is.EqualTo(model.FirstName));
			Assert.That(updatedReservation.LastName, Is.EqualTo(model.LastName));
			Assert.That(updatedReservation.PhoneNumber, Is.EqualTo(model.PhoneNumber));
			Assert.That(updatedReservation.Email, Is.EqualTo(model.Email));
			Assert.That(updatedReservation.PeopleCount, Is.EqualTo(model.PeopleCount));
			Assert.That(updatedReservation.Description, Is.EqualTo(model.Description));
			Assert.That(updatedReservation.Date.ToString("g"), Is.EqualTo(model.Date));
		}

		[Test]
		public async Task EditReservationAsync_ShouldDecreaseReservationPeopleCount_WhenPeopleCountIsDifferent()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var date = DateTime.Now.AddDays(1);

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 95,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = date,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});

			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = date.ToString("g"),
				PeopleCount = 2,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			var updatedReservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == Guid.Parse(reservationId));
			Assert.That(updatedReservation!.PeopleCount, Is.EqualTo(2));
		}

		[Test]
		public async Task EditReservationAsync_ShouldIncreaseReservationPeopleCount_WhenPeopleCountIsDifferent()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var date = DateTime.Now.AddDays(1);

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 60,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = date,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 2,
						Description = "New Year party reservation"
					}
				}
			});

			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = date.ToString("g"),
				PeopleCount = 5,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			var updatedReservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == Guid.Parse(reservationId));
			Assert.That(updatedReservation!.PeopleCount, Is.EqualTo(5));
		}

		[Test]
		public async Task EditReservationAsync_ShouldReturnAvailableCapacity_WhenPeopleCountIsBiggerThanTotalCapaciti()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var date = DateTime.Now.AddDays(1);

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 20,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = date,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 2,
						Description = "New Year party reservation"
					}
				}
			});

			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = date.ToString("g"),
				PeopleCount = 30,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			var updatedReservation = await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == Guid.Parse(reservationId));
			Assert.That(updatedReservation!.PeopleCount, Is.EqualTo(2));
			Assert.That(result, Is.EqualTo("There are 22 spaces available for booking."));
		}

		[Test]
		public async Task EditReservationAsync_ShouldReturnErrorMessage_WhenDateIsBeforeToday()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var yesterday = DateTime.Now.AddDays(-1);

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 95,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = DateTime.Now,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});
			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = yesterday.ToString(),
				PeopleCount = 10,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			Assert.That(result, Is.EqualTo("You have enter invalid date!"));
		}

		[Test]
		public async Task EditReservationAsync_ShouldReturnErrorMessage_WhenPeopleCountIsBelowRange()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var date = DateTime.Now;

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 95,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = date,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});
			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = date.ToString(),
				PeopleCount = 0,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			Assert.That(result, Is.EqualTo("The number of guests must be between 1 and 60."));
		}

		[Test]
		public async Task EditReservationAsync_ShouldReturnErrorMessage_WhenPeopleCountIsAboveRange()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var date = DateTime.Now;

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 95,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = date,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});
			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = date.ToString(),
				PeopleCount = 61,
				Description = "Updated Description"
			};

			var result = await reservationService.EditReservationAsync(model, userId, reservationId);

			Assert.That(result, Is.EqualTo("The number of guests must be between 1 and 60."));
		}

		[Test]
		public async Task EditReservationAsync_ShouldThrowExceptionWhenReservationNotExist()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var tomorow = DateTime.Now.AddDays(1);

			await dbContext.CapacitySlots.AddAsync(new CapacitySlot
			{
				SlotDate = DateTime.UtcNow.AddDays(5),
				CurrentCapacity = 95,
				Reservations = new List<Reservation>()
				{
					new Reservation
					{
						Id = Guid.Parse(reservationId),
						ApplicationUserId = Guid.Parse(userId),
						Date = DateTime.Now,
						FirstName = "FirstName",
						LastName = "LastName",
						PhoneNumber = "123456789",
						Email = "test@example.com",
						PeopleCount = 5,
						Description = "New Year party reservation"
					}
				}
			});
			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = tomorow.ToString(),
				PeopleCount = 10,
				Description = "Updated Description"
			};

			try
			{
				await reservationService.EditReservationAsync(model, userId, Guid.NewGuid().ToString());
			}
			catch (Exception ex)
			{
				Assert.That(ex, Is.Not.Null);
				Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'currentReservation')"));
			}
		}

		[Test]
		public async Task EditReservationAsync_ShouldThrowExceptionWhenUserNotExist()
		{
			var userId = Guid.NewGuid().ToString();
			var reservationId = Guid.NewGuid().ToString();
			var tomorow = DateTime.Now.AddDays(1);

			var reservation = new Reservation
			{
				Id = Guid.Parse(reservationId),
				FirstName = "FirstName",
				LastName = "LastName",
				PhoneNumber = "123456789",
				Email = "email@example.com",
				Date = DateTime.Now,
				PeopleCount = 5,
				Description = "Description",
				ApplicationUserId = Guid.Parse(userId)

			};

			await dbContext.Reservations.AddAsync(reservation);
			await dbContext.SaveChangesAsync();

			var model = new ReservationFormModel
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				PhoneNumber = "987654321",
				Email = "update@example.com",
				Date = tomorow.ToString(),
				PeopleCount = 10,
				Description = "Updated Description"
			};


			var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await reservationService.EditReservationAsync(model, Guid.NewGuid().ToString(), reservationId));

			Assert.That(ex, Is.Not.Null);
			Assert.That(ex.ParamName, Is.EqualTo("currentReservation"));
		}

		//GetAllFullyBookedDatesReservationAsync
		[Test]
		public async Task GetAllFullyBookedDatesReservationAsync_ShouldReturnAllDates()
		{
			var date = DateTime.UtcNow.AddDays(5);

			await dbContext.CapacitySlots.AddAsync(
				new CapacitySlot()
				{
					CurrentCapacity = 0,
					SlotDate = date,
				});

			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllFullyBookedDatesInReservationAsync();

			Assert.That(result, Is.Not.Null);
			Assert.That(1, Is.EqualTo(result.Count()));
		}

		[Test]
		public async Task GetAllFullyBookedDatesReservationAsync_ShouldReturnZeroCount()
		{
			var date = DateTime.UtcNow.AddDays(5);

			await dbContext.CapacitySlots.AddAsync(
				new CapacitySlot()
				{
					CurrentCapacity = 5,
					SlotDate = date,
				});

			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllFullyBookedDatesInReservationAsync();

			Assert.That(result, Is.Not.Null);
			Assert.That(0, Is.EqualTo(result.Count()));
		}

		//GetAllReservationsAsync
		[Test]
		public async Task GetAllReservationsAsync_ShouldReturnAllReservationsWithoutAnyFilters()
		{
			var userId = Guid.NewGuid();

			await dbContext.Reservations.AddRangeAsync(new List<Reservation>()
			{
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName1",
					LastName = "LastName1",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 4,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName2",
					LastName = "LastName2",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 5,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(3),
					FirstName = "FirstName3",
					LastName = "LastName3",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 6,
					Description = "Description 1",
					CapacitySlotId = 1
				}
			});

			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllReservationsAsync(null, null, null, null);

			Assert.That(result, Is.Not.Null);
			Assert.That(3, Is.EqualTo(result.Reservations.Count()));
		}

		[Test]
		public async Task GetAllReservationsAsync_ShouldReturnAllReservationsByFiteredCorectDates()
		{
			var userId = Guid.NewGuid();
			int? pageNumber = 1;
			DateTime? startDate = DateTime.Now.AddDays(1);
			DateTime? endDate = DateTime.Now.AddDays(2);
			string? name = null;

			await dbContext.Reservations.AddRangeAsync(new List<Reservation>()
			{
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName1",
					LastName = "LastName1",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 4,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName2",
					LastName = "LastName2",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 5,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(3),
					FirstName = "FirstName3",
					LastName = "LastName3",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 6,
					Description = "Description 1",
					CapacitySlotId = 1
				}
			});

			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllReservationsAsync(pageNumber, startDate, endDate, name);

			Assert.That(result, Is.Not.Null);
			Assert.That(2, Is.EqualTo(result.Reservations.Count()));
		}

		[Test]
		public async Task GetAllReservationsAsync_ShouldReturnAllReservationsByName()
		{
			var userId = Guid.NewGuid();
			int? pageNumber = 1;
			string? name = "FirstName1";

			await dbContext.Reservations.AddRangeAsync(new List<Reservation>()
			{
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName1",
					LastName = "LastName1",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 4,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName2",
					LastName = "LastName2",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 5,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(3),
					FirstName = "FirstName3",
					LastName = "LastName3",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 6,
					Description = "Description 1",
					CapacitySlotId = 1
				}
			});

			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllReservationsAsync(pageNumber, null, null, name);

			Assert.That(result, Is.Not.Null);
			Assert.That(1, Is.EqualTo(result.Reservations.Count()));
			Assert.That("FirstName1 LastName1", Is.EqualTo(result.Reservations.First().Name));
		}

		[Test]
		public async Task GetAllReservationsAsync_ShouldReturnAllReservationsOrderedByDateAscending()
		{
			var userId = Guid.NewGuid();
			int? pageNumber = 1;

			await dbContext.Reservations.AddRangeAsync(new List<Reservation>()
			{
				new Reservation
				{
					Date = DateTime.Now.AddDays(3),
					FirstName = "FirstName3",
					LastName = "LastName3",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 4,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(1),
					FirstName = "FirstName1",
					LastName = "LastName1",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 5,
					Description = "Description 1",
					CapacitySlotId = 1
				},
				new Reservation
				{
					Date = DateTime.Now.AddDays(2),
					FirstName = "FirstName2",
					LastName = "LastName2",
					PhoneNumber = "0987654323",
					Email = "test@gmail.com",
					ApplicationUserId = userId,
					PeopleCount = 6,
					Description = "Description 1",
					CapacitySlotId = 1
				}
			});

			await dbContext.SaveChangesAsync();

			var result = await reservationService.GetAllReservationsAsync(pageNumber, null, null, null);

			Assert.That(result, Is.Not.Null);
			Assert.That(3, Is.EqualTo(result.Reservations.Count()));
			Assert.That("FirstName1 LastName1", Is.EqualTo(result.Reservations.First().Name));
			Assert.That("FirstName3 LastName3", Is.EqualTo(result.Reservations.Last().Name));
		}

		[Test]
		public async Task GetAllReservationsAsync_ShouldReturnInvalidOperationExceptionWhenStartDateIsAfterEndDate()
		{
			DateTime? startDate = DateTime.Now.AddDays(2);
			DateTime? endDate = DateTime.Now.AddDays(1);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
               await reservationService.GetAllReservationsAsync(null, startDate, endDate, null));

            Assert.That(ex.Message, Is.EqualTo("Invalid date range"));
        }

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose();
		}


	}
}
