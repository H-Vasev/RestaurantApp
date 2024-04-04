using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Menu;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Configurations;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
    public class ProductServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext dbContext;
        private IProductService productService;

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

            productService = new ProductService(dbContext);
        }


        //AddProductAsync
        [Test]
        public async Task AddProductAsync_ShouldAddProductToDatabase()
        {
            var product = new ProductFormModel()
            {
                Title = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = 1,
            };

            await productService.AddProductAsync(product);

            var productFromDb = await dbContext.Products.FindAsync(1);

            Assert.That(product.Title, Is.EqualTo(productFromDb.Name));
            Assert.That(product.Description, Is.EqualTo(productFromDb.Description));
            Assert.That(product.Price, Is.EqualTo(productFromDb.Price));
            Assert.That(product.CategoryId, Is.EqualTo(productFromDb.CategoryId));
        }

        [Test]
        public async Task AddProductAsync_ShouldThrowException_WhenProductIsNull()
        {
            ProductFormModel product = null;

            try
            {
                await productService.AddProductAsync(product);
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentNullException>());
            }
        }

        //GetProductByIdForEditAsync
        [Test]
        public async Task GetProductByIdForEditAsync_ShouldReturnProductFormModel()
        {
            var product = new Product()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = 2,
                Image = "test.jpg",
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var productFormModel = await productService.GetProductByIdForEditAsync(1);

            Assert.That(product.Id, Is.EqualTo(productFormModel.Id));
            Assert.That(product.Name, Is.EqualTo(productFormModel.Title));
            Assert.That(product.Description, Is.EqualTo(productFormModel.Description));
            Assert.That(product.Price, Is.EqualTo(productFormModel.Price));
            Assert.That(product.CategoryId, Is.EqualTo(productFormModel.CategoryId));
            Assert.That(product.Image, Is.EqualTo(productFormModel.ImagePath));
        }

        [Test]
        public async Task GetProductByIdForEditAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            try
            {
                var productFormModel = await productService.GetProductByIdForEditAsync(1);

            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentNullException>());
            }

        }

        //GetProductImagePathAsync
        [Test]
        public async Task GetProductImagePathAsync_ShouldReturnProductImagePath()
        {
            var product = new Product()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = 2,
                Image = "test.jpg",
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var imagePath = await productService.GetProductImagePathAsync(1);

            Assert.That(product.Image, Is.EqualTo(imagePath));
        }

        [Test]
        public async Task GetProductImagePathAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            try
            {
                var imagePath = await productService.GetProductImagePathAsync(1);

            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentNullException>());
            }

        }

        //EditProductAsync
        [Test]
        public async Task EditProductAsync_ShouldEditProductInDatabase()
        {
            var product = new Product()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = 2,
                Image = "test.jpg",
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var productFormModel = new ProductFormModel()
            {
                Id = 1,
                Title = "Edited Product",
                Description = "Edited Description",
                Price = 20.00m,
                CategoryId = 3,
            };

            await productService.EditProductAsync(productFormModel, 1);

            var editedProduct = await dbContext.Products.FindAsync(1);

            Assert.That(productFormModel.Title, Is.EqualTo(editedProduct.Name));
            Assert.That(productFormModel.Description, Is.EqualTo(editedProduct.Description));
            Assert.That(productFormModel.Price, Is.EqualTo(editedProduct.Price));
            Assert.That(productFormModel.CategoryId, Is.EqualTo(editedProduct.CategoryId));
        }

        [Test]
        public async Task EditProductAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var productFormModel = new ProductFormModel()
            {
                Id = 1,
                Title = "Edited Product",
                Description = "Edited Description",
                Price = 20.00m,
                CategoryId = 3,
            };

            try
            {
                await productService.EditProductAsync(productFormModel, 1);
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentNullException>());
            }
        }

        //RemoveProductAsync
        [Test]
        public async Task RemoveProductAsync_ShouldRemoveProductFromDatabase()
        {
            var product = new Product()
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = 2,
                Image = "test.jpg",
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            await productService.RemoveProductAsync(1);

            var productFromDb = await dbContext.Products.FindAsync(1);

            Assert.That(productFromDb, Is.Null);
        }

        [Test]
        public async Task RemoveProductAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            try
            {
                await productService.RemoveProductAsync(1);
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ArgumentNullException>());
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
