using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Configurations;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
    public class MenuServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext dbContext;
        private IMenuService menuService;

        private Mock<IMemoryCache> mockCache;
        private Dictionary<object, object> fakeCacheStore = new Dictionary<object, object>();

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

            mockCache = new Mock<IMemoryCache>();
            mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns((object key) =>
            {
                var mockEntry = new Mock<ICacheEntry>();
                mockEntry.SetupSet(m => m.Value = It.IsAny<object>()).Callback<object>(value => fakeCacheStore[key] = value);
                return mockEntry.Object;
            });

            menuService = new MenuService(dbContext, mockCache.Object);
        }

        //GetCategoriesAsync
        [Test]
        public async Task GetCategoriesAsync_ShouldReturnAllCategories()
        {
            var categories = new List<Category>
             {
                 new Category { Id = 1, CategoryName = "Category 1" },
                 new Category { Id = 2, CategoryName = "Category 2" },
                 new Category { Id = 3, CategoryName = "Category 3" }
             };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();

            var cacheKey = "categories";
            object cachedValue;
            var result = await menuService.GetCategoriesAsync();

            Assert.That(fakeCacheStore.TryGetValue(cacheKey, out cachedValue), Is.True);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.First().Name, Is.EqualTo("Category 1"));
        }

        [Test]
        public async Task GetCategoriesAsync_ShouldReturnZeroCount()
        {
            var result = await menuService.GetCategoriesAsync();

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        //GetMenuAsync
        [Test]
        public async Task GetMenuAsync_ShouldReturnAllProducts()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "Category 1" },
                new Category { Id = 2, CategoryName = "Category 2" },
                new Category { Id = 3, CategoryName = "Category 3" }
            };

            dbContext.Categories.AddRange(categories);

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 2 },
                new Product { Id = 3, Name = "Product 3", Price = 30, CategoryId = 3 }
            };

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            var result = await menuService.GetMenuAsync(null);

            Assert.That(3, Is.EqualTo(result.Count()));

            Assert.That("Product 1", Is.EqualTo(result.First().Name));
            Assert.That(10, Is.EqualTo(result.First().Price));
            Assert.That(1, Is.EqualTo(result.First().CategoryId));
            
            Assert.That("Product 2", Is.EqualTo(result.ElementAt(1).Name));
            Assert.That(20, Is.EqualTo(result.ElementAt(1).Price));
            Assert.That(2, Is.EqualTo(result.ElementAt(1).CategoryId));

            Assert.That("Product 3", Is.EqualTo(result.Last().Name));
            Assert.That(30, Is.EqualTo(result.Last().Price));
            Assert.That(3, Is.EqualTo(result.Last().CategoryId));

        }

        [Test]
        public async Task GetMenuAsync_ShouldReturnAllProductsByCategoryName()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "Category 1" },
                new Category { Id = 2, CategoryName = "Category 2" },
                new Category { Id = 3, CategoryName = "Category 3" }
            };

            dbContext.Categories.AddRange(categories);

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 },
                new Product { Id = 3, Name = "Product 3", Price = 30, CategoryId = 3 }
            };

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            var result = await menuService.GetMenuAsync("Category 1");

            Assert.That(2, Is.EqualTo(result.Count()));
            Assert.That("Product 1", Is.EqualTo(result.First().Name));
            Assert.That(10, Is.EqualTo(result.First().Price));
            Assert.That(1, Is.EqualTo(result.First().CategoryId));

            Assert.That("Product 2", Is.EqualTo(result.Last().Name));
            Assert.That(20, Is.EqualTo(result.Last().Price));
            Assert.That(1, Is.EqualTo(result.Last().CategoryId));
        }

        [Test]
        public async Task GetMenuAsync_ShouldReturnZeroCount()
        {
            var result = await menuService.GetMenuAsync("Category 1");

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        //GetProductByIdAsync
        [Test]
        public async Task GetProductByIdAsync_ShouldReturnProductById()
        {
            var product = new Product ()
            {
                Id = 1,
                Name = "Product 1", 
                Price = 10,
                CategoryId = 1 
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var result = await menuService.GetProductByIdAsync(1);

            Assert.That(1, Is.EqualTo(result.Id));
            Assert.That("Product 1", Is.EqualTo(result.Name));
            Assert.That(10, Is.EqualTo(result.Price));
            Assert.That(1, Is.EqualTo(result.CategoryId));
        }

        [Test]
        public void GetProductByIdAsync_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await menuService.GetProductByIdAsync(1));
        }

        [TearDown]
        public void TearDown()
        {
            DatabaseSeedController.SeedEnabled = true;

            dbContext.Dispose();
        }
    }
}
