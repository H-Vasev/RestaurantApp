

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Configurations;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
    public class EventServiceTests
    {
        private DbContextOptions<ApplicationDbContext> dbContextOptions;
        private ApplicationDbContext dbContext;
        private IEventService eventService;

        [SetUp]
        public void Setup()
        {
            DatabaseSeedController.SeedEnabled = false;

            dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new ApplicationDbContext(dbContextOptions);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            eventService = new EventService(dbContext);
        }

        //GetAllEventsAsync
        [Test]
        public async Task GetAllEventsAsync_ShouldReturnAllFutureEvents()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(-1)
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2)
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(3)
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllEventsAsync();

            Assert.That(2, Is.EqualTo(result.Count()));
            Assert.That(2, Is.EqualTo(result.First().Id));
            Assert.That("Event 2", Is.EqualTo(result.First().Title));
        }


        [Test]
        public async Task GetAllEventsAsync_ShouldReturnZeroCount()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(-1)
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(-2)
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(-3)
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllEventsAsync();

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetAllEventsAsync_ShouldReturnOrderedByStartDate()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(2),
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(3)
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(1)
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllEventsAsync();

            Assert.That(3, Is.EqualTo(result.ElementAt(0).Id));
            Assert.That(1, Is.EqualTo(result.ElementAt(1).Id));
            Assert.That(2, Is.EqualTo(result.ElementAt(2).Id));
        }


        [Test]
        public async Task GetAllEventsAsync_ShouldReturnAllEqualDates()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllEventsAsync();

            Assert.That(3, Is.EqualTo(result.Count()));
        }

        //GetEventByIdAsync
        [Test]
        public async Task GetEventByIdAsync_ShouldReturnEventById()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(1)
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2)
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(3)
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetEventByIdAsync(2);

            Assert.That(2, Is.EqualTo(result.Id));
            Assert.That("Event 2", Is.EqualTo(result.Title));
        }

        [Test]
        public async Task GetEventByIdAsync_ShouldReturnNull()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(1)
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2)
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(3)
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetEventByIdAsync(4);

            Assert.That(result, Is.Null);
        }


        [Test]
        public async Task GetEventByIdAsync_ShouldReturnNullWhenIdIsNegative()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(1)
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2)
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(3)
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetEventByIdAsync(-4);

            Assert.That(result, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            DatabaseSeedController.SeedEnabled = true;
            dbContext.Dispose();
        }

    }
}
