using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Packaging.Rules;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.ShoppingCart;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Configurations;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
    public class OrderServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext dbContext;
        private IOrderService orderService;
        private Mock<IShoppingCartService> shoppingCartServiceMock;

        [SetUp]
        public void Setup()
        {
            DatabaseSeedController.SeedEnabled = false;

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new ApplicationDbContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            orderService = new OrderService(dbContext, null);
        }

        //GetOrdersAsync
        [Test]
        public async Task GetOrdersAsync_WithOrders_ShouldReturnOrdersWithNoItems()
        {
            var userId = Guid.NewGuid();
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                TotalPrice = 100,
                UserId = userId,
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var result = await orderService.GetOrdersAsync(userId.ToString());
            var orderItems = result.First().OredrItemsViewModel;

            Assert.That(1, Is.EqualTo(result.Count()));
            Assert.That(0, Is.EqualTo(orderItems.Count()));
        }

        [Test]
        public async Task GetOrdersAsync_WithOrders_ShouldReturnOrdersWithItems()
        {
            var userId = Guid.NewGuid();
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                TotalPrice = 100,
                UserId = userId,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        ProductId = 1,
                        Quantity = 1,
                        Price = 10
                    },
                    new OrderItem()
                    {
                        ProductId = 2,
                        Quantity = 2,
                        Price = 20
                    }
                }
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var result = await orderService.GetOrdersAsync(userId.ToString());
            var orderItems = result.First().OredrItemsViewModel;
            var orderItem1 = orderItems.First().ProductId;
            var orderItem2 = orderItems.Last().ProductId;

            Assert.That(1, Is.EqualTo(result.Count()));
            Assert.That(1, Is.EqualTo(orderItem1));
            Assert.That(2, Is.EqualTo(orderItem2));
            Assert.That(2, Is.EqualTo(orderItems.Count()));
        }

        [Test]
        public async Task GetOrdersAsync_WithNoOrders_ShouldReturnEmptyList()
        {
            var userId = Guid.NewGuid();

            var result = await orderService.GetOrdersAsync(userId.ToString());

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetOrdersAsync_WithOrders_ShouldReturnOrderedByDescending()
        {
            var userId = Guid.NewGuid();
            var firstOrderId = Guid.NewGuid();
            var secondOrderId = Guid.NewGuid();

            var orders = new List<Order>()
            {
                 new Order()
                 {
                     Id = firstOrderId,
                     OrderDate = DateTime.Now.AddDays(-2),
                     TotalPrice = 100,
                     UserId = userId,
                     OrderItems = new List<OrderItem>()
                     {
                        new OrderItem()
                        {
                            ProductId = 1,
                            Quantity = 1,
                            Price = 10
                        },
                        new OrderItem()
                        {
                            ProductId = 2,
                            Quantity = 2,
                            Price = 20
                        }
                     }
                 },
                 new Order()
                 {
                        Id = secondOrderId,
                        OrderDate = DateTime.Now.AddDays(-1),
                        TotalPrice = 200,
                        UserId = userId,
                        OrderItems = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                ProductId = 3,
                                Quantity = 1,
                                Price = 30
                            },
                            new OrderItem()
                            {
                                ProductId = 4,
                                Quantity = 2,
                                Price = 40
                            }
                        }
                    }
            };



            await dbContext.Orders.AddRangeAsync(orders);
            await dbContext.SaveChangesAsync();

            var result = await orderService.GetOrdersAsync(userId.ToString());

            var firstItem = result.First();
            var secondItem = result.Last();

            Assert.That(Guid.Parse(secondItem.Id), Is.EqualTo(firstOrderId));
            Assert.That(Guid.Parse(firstItem.Id), Is.EqualTo(secondOrderId));
            Assert.That(2, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetOrdersAsync_WithMoreThanThreeOrders_ShouldReturnLastThreeOrders()
        {
            var userId = Guid.NewGuid();

            var orders = new List<Order>()
            {
                 new Order()
                 {
                     OrderDate = DateTime.Now.AddDays(-4),
                     TotalPrice = 100,
                     UserId = userId,
                     OrderItems = new List<OrderItem>()
                     {
                        new OrderItem()
                        {
                            ProductId = 1,
                            Quantity = 1,
                            Price = 10
                        },
                        new OrderItem()
                        {
                            ProductId = 2,
                            Quantity = 2,
                            Price = 20
                        }
                     }
                 },
                 new Order()
                 {
                        OrderDate = DateTime.Now.AddDays(-3),
                        TotalPrice = 200,
                        UserId = userId,
                        OrderItems = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                ProductId = 3,
                                Quantity = 1,
                                Price = 30
                            },
                            new OrderItem()
                            {
                                ProductId = 4,
                                Quantity = 2,
                                Price = 40
                            }
                        }
                    },
                 new Order()
                 {
                        OrderDate = DateTime.Now.AddDays(-2),
                        TotalPrice = 300,
                        UserId = userId,
                        OrderItems = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                ProductId = 5,
                                Quantity = 1,
                                Price = 50
                            },
                            new OrderItem()
                            {
                                ProductId = 6,
                                Quantity = 2,
                                Price = 60
                            }
                        }
                    },
                 new Order()
                 {
                        OrderDate = DateTime.Now.AddDays(-1),
                        TotalPrice = 400,
                        UserId = userId,
                        OrderItems = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                ProductId = 7,
                                Quantity = 1,
                                Price = 70
                            },
                            new OrderItem()
                            {
                                ProductId = 8,
                                Quantity = 2,
                                Price = 80
                            }
                        }
                    }
            };

            await dbContext.Orders.AddRangeAsync(orders);
            await dbContext.SaveChangesAsync();

            var result = await orderService.GetOrdersAsync(userId.ToString());

            Assert.That(3, Is.EqualTo(result.Count()));
        }

        //GetDataForCheckoutAsync
        [Test]
        public async Task GetDataForCheckoutAsync_WithValidUserId_ShouldReturnCheckoutModel()
        {
            shoppingCartServiceMock = new Mock<IShoppingCartService>();
            shoppingCartServiceMock.Setup( x => x.GetAllItemsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ShoppingCartViewModel>()
                {
                    new ShoppingCartViewModel()
                    {
                        ProductId = 1,
                        Quantity = 1,
                        Price = 10m,
                        ProductName = "Product1",
                        Image = "Image1",
                    },
                    new ShoppingCartViewModel()
                    {
                        ProductId = 2,
                        Quantity = 2,
                        Price = 20m,
                        ProductName = "Product2",
                        Image = "Image2"
                    }
                });

            var userId = Guid.NewGuid();
            dbContext.Users.Add(new ApplicationUser
            {
                Id = userId,
                FirsName = "Firstname",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Address = new Address { Street = "123 Main St", PostalCode = "12345" }
            });

            await dbContext.SaveChangesAsync();

            orderService = new OrderService(dbContext, shoppingCartServiceMock.Object);

            var result = await orderService.GetDataForCheckoutAsync(userId.ToString());

            Assert.That(2, Is.EqualTo(result.Items.Count()));
            Assert.That(50, Is.EqualTo(result.TotalPrice));
            Assert.That("Firstname", Is.EqualTo(result.Name));
            Assert.That("123 Main St", Is.EqualTo(result.Address));
            Assert.That("12345", Is.EqualTo(result.PostalCode));
            Assert.That("test@example.com", Is.EqualTo(result.Email));
        }

        [Test]
        public void GetDataForCheckoutAsync_WithInvalidUserId_ShouldThrowArgumentException()
        {
            var userId = Guid.NewGuid();

            Assert.ThrowsAsync<ArgumentException>(() => orderService.GetDataForCheckoutAsync(userId.ToString()));
        }

        //CheckoutAsync
        [Test]
        public async Task CheckoutAsync_WithValidModel_ShouldAddOrderToDb()
        {
            var userId = Guid.NewGuid();
            var model = new CheckoutFormModel()
            {
                Items = new List<ShoppingCartViewModel>()
                {
                    new ShoppingCartViewModel()
                    {
                        ProductId = 1,
                        Quantity = 1,
                        Price = 10m,
                        ProductName = "Product1",
                        Image = "Image1",
                    },
                    new ShoppingCartViewModel()
                    {
                        ProductId = 2,
                        Quantity = 2,
                        Price = 20m,
                        ProductName = "Product2",
                        Image = "Image2"
                    }
                },
                TotalPrice = 50
            };

            dbContext.Users.Add(new ApplicationUser
            {
                Id = userId,
                FirsName = "Firstname",
                Email = "test@gmail.com",
                PhoneNumber = "1234567890",
            });
            await dbContext.SaveChangesAsync();

            await orderService.CheckoutAsync(model, userId.ToString());

            var order = await dbContext.Orders.FirstOrDefaultAsync();

            Assert.That(50, Is.EqualTo(order.TotalPrice));
            Assert.That(userId, Is.EqualTo(order.UserId));
            Assert.That(2, Is.EqualTo(order.OrderItems.Count));
        }

        [Test]
        public async Task CheckoutAsync_ShouldThrowExeption()
        {
            var userId = Guid.NewGuid();
            var model = new CheckoutFormModel()
            {
                Items = new List<ShoppingCartViewModel>(),
                TotalPrice = 0
            };

            dbContext.Users.Add(new ApplicationUser
            {
                Id = userId,
                FirsName = "Firstname",
                Email = "test@gmail.com",
                PhoneNumber = "1234567890",
            });
            await dbContext.SaveChangesAsync();

            try
            {
                await orderService.CheckoutAsync(model, userId.ToString());
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentException>());
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
