using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
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
            Assert.That("Date must be bigger than today!", Is.EqualTo(result));
        }

        [Test]
        public async Task AddReservationAsync_ShouldReturnError_WhenDateIsEqualToToday()
        {
            var date = DateTime.UtcNow.ToString("g");
            var model2 = new ReservationFormModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Date = date,
                PeopleCount = 4,
                Description = "Birthday party"
            };

            var result2 = await reservationService.AddReservationAsync(model2, Guid.NewGuid().ToString(), 1);

            Assert.That(result2, Is.Not.EqualTo(string.Empty));
            Assert.That("Date must be bigger than today!", Is.EqualTo(result2));
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
                ApplicationUserId = Guid.Parse(userId)
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

            Assert.That("You have already made a reservation for this date please check your Reservation!", Is.EqualTo(result));
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

            Assert.That("The number of people must be between 1 and 60", Is.EqualTo(result));
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

            Assert.That("The number of people must be between 1 and 60", Is.EqualTo(result));
        }

        [Test]
        public async Task AddReservationAsync_ShouldReturnEmptyString_WhenPeopleCountIsInRange()
        {
            var model = new ReservationFormModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Date = DateTime.Now.AddDays(5).ToString("g"),
                PeopleCount = 20,
                Description = "Birthday party"
            };

            var result = await reservationService.AddReservationAsync(model, Guid.NewGuid().ToString(), 1);

            Assert.That(result, Is.EqualTo(string.Empty));
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

            var result = await reservationService.GetAllReservationAsync(userId);

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

            var result = await reservationService.GetAllReservationAsync(Guid.NewGuid().ToString());

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

            var result = await reservationService.GetAllReservationAsync(userId);

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

            var result = await reservationService.GetAllReservationAsync(userId);

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

            var result = await reservationService.GetAllReservationAsync(userId);

            Assert.That(3, Is.EqualTo(result.Count()));
            Assert.That("Second", Is.EqualTo(result.ElementAt(0).FirstName));
            Assert.That("First", Is.EqualTo(result.ElementAt(1).FirstName));
            Assert.That("Third", Is.EqualTo(result.ElementAt(2).FirstName));
        }


        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


    }
}
