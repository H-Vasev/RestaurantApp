

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Event;
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

        private Mock<IMemoryCache> memoryCache;
        private Dictionary<object, object> cacheDictionary = new Dictionary<object, object>();

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

            memoryCache = new Mock<IMemoryCache>();
            memoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns((object key) =>
            {
				var mockEntry = new Mock<ICacheEntry>();
				mockEntry.SetupSet(m => m.Value = It.IsAny<object>()).Callback<object>(value => cacheDictionary[key] = value);
				return mockEntry.Object;
			});

            eventService = new EventService(dbContext, memoryCache.Object);
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

        //GetAllBoockedEventIdsAsync
        [Test]
        public async Task GetAllBoockedEventIdsAsync_ShouldReturnAllBookedEvents()
        {
            var userId = Guid.NewGuid();
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(1),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId
                        }
                    }
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId
                        }
                    }
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(3),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId,
                        }
                    }
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllBoockedEventIdsAsync(userId.ToString());

            Assert.That(3, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetAllBoockedEventIdsAsync_ShouldReturnAllBookedEventsForUser()
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(1),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId1
                        }
                    }
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId2
                        }
                    }
                },
                new Event
                {
                    Id = 3,
                    Title = "Event 3",
                    Description = "Description 3",
                    StartEvent = DateTime.Now.AddDays(3),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId2
                        }
                    }
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllBoockedEventIdsAsync(userId2.ToString());

            Assert.That(2, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetAllBoockedEventIdsAsync_ShouldReturnZeroCount()
        {
            var userId = Guid.NewGuid();
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartEvent = DateTime.Now.AddDays(1),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId
                        }
                    }
                },
                new Event
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartEvent = DateTime.Now.AddDays(2),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ApplicationUserId = userId
                        }
                    }
                },
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            var result = await eventService.GetAllBoockedEventIdsAsync(Guid.NewGuid().ToString());

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        //AddEventAsync
        [Test]
        public async Task AddEventAsync_ShouldAddEventSuccessfully()
        {
            var model = new EventFormModel
            {
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(1),
                EndEvent = DateTime.Now.AddDays(2)
            };

            await eventService.AddEventAsync(model);

            var result = await dbContext.Events.FirstOrDefaultAsync();

            Assert.That(1, Is.EqualTo(result.Id));
            Assert.That("Event 1", Is.EqualTo(result.Title));
        }

        [Test]
        public async Task AddEventAsync_ShouldAddEventSuccessfullyWhenDateIsEqual()
        {
            var model = new EventFormModel
            {
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(1),
                EndEvent = DateTime.Now.AddDays(1)
            };

            await eventService.AddEventAsync(model);

            var result = await dbContext.Events.FirstOrDefaultAsync();

            Assert.That(1, Is.EqualTo(result.Id));
            Assert.That("Event 1", Is.EqualTo(result.Title));
        }

        [Test]
        public async Task AddEventAsync_ShouldThrowExceptionWhenEndDateIsBeforeStartDate()
        {
            var model = new EventFormModel
            {
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(2),
                EndEvent = DateTime.Now.AddDays(1)
            };

            try
            {
                await eventService.AddEventAsync(model);
            }
            catch (Exception ex)
            {
                Assert.That("Start date must be bigger than end date!", Is.EqualTo(ex.Message));
            }
        }

        //GetEventByIdForEditAsync
        [Test]
        public async Task GetEventByIdForEditAsync_ShouldReturnEventById()
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

            var result = await eventService.GetEventByIdForEditAsync(2);

            Assert.That("Event 2", Is.EqualTo(result.Title));
            Assert.That("Description 2", Is.EqualTo(result.Description));
        }

        [Test]
        public async Task GetEventByIdForEditAsync_ShouldThrowExceptionWhenEventIsNull()
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
            };

            await dbContext.Events.AddRangeAsync(events);
            await dbContext.SaveChangesAsync();

            try
            {
                await eventService.GetEventByIdForEditAsync(4);
            }
            catch (Exception ex)
            {
                Assert.That("Value cannot be null. (Parameter 'ev')", Is.EqualTo(ex.Message));
            }
        }

        //EditEventAsync
        [Test]
        public async Task EditEventAsync_ShouldEditEventSuccessfully()
        {
            var model = new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(1),
                EndEvent = DateTime.Now.AddDays(2),
            };

            await dbContext.AddAsync(model);

            var result = await dbContext.Events.FindAsync(1);

            Assert.That(1, Is.EqualTo(result.Id));
            Assert.That("Event 1", Is.EqualTo(result.Title));
            Assert.That("Description 1", Is.EqualTo(result.Description));


            var editModel = new EventFormModel
            {
                Title = "Event 2",
                Description = "Description 2",
                StartEvent = DateTime.Now.AddDays(2),
                EndEvent = DateTime.Now.AddDays(3)
            };

            await eventService.EditEventAsync(editModel, 1);

            var editedResult = await dbContext.Events.FirstOrDefaultAsync();

            Assert.That(1, Is.EqualTo(editedResult.Id));
            Assert.That("Event 2", Is.EqualTo(editedResult.Title));
            Assert.That("Description 2", Is.EqualTo(editedResult.Description));
        }

        [Test]
        public async Task EditEventAsync_ShouldThrowExceptionWhenEventIsNull()
        {
            var model = new EventFormModel
            {
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(1),
                EndEvent = DateTime.Now.AddDays(2)
            };

            await eventService.AddEventAsync(model);

            try
            {
                await eventService.EditEventAsync(model, 2);
            }
            catch (Exception ex)
            {
                Assert.That("Value cannot be null. (Parameter 'ev')", Is.EqualTo(ex.Message));
            }

        }

        //RemoveEventAsync
        [Test]
        public async Task RemoveEventAsync_ShouldRemoveEventSuccessfully()
        {
            var model = new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(1),
                EndEvent = DateTime.Now.AddDays(2),
            };  

            await dbContext.Events.AddAsync(model);

            var modelToRemove = await dbContext.Events.FindAsync(1);

            await eventService.RemoveEventAsync(modelToRemove.Id);

            var removedResult = await dbContext.Events.FindAsync(1);

            Assert.That(removedResult, Is.Null);
        }

        [Test]
        public async Task RemoveEventAsync_ShouldThrowExceptionWhenEventIsNull()
        {
            var model = new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = "Description 1",
                StartEvent = DateTime.Now.AddDays(1),
                EndEvent = DateTime.Now.AddDays(2),
            };

            await dbContext.AddAsync(model);

            try
            {
                await eventService.RemoveEventAsync(2);
            }
            catch (Exception ex)
            {
                Assert.That("Value cannot be null. (Parameter 'ev')", Is.EqualTo(ex.Message));
            }
        }

        [TearDown]
        public void TearDown()
        {
            DatabaseSeedController.SeedEnabled = true;
            dbContext.Dispose();
        }

    }
}
