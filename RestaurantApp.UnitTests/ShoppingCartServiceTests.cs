using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Configurations;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
    public class ShoppingCartServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext dbContext;
        private IShoppingCartService shoppingCartService;

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

            shoppingCartService = new ShoppingCartService(dbContext);
        }

        //GetItamsQuantityAsync
        [Test]
        public async Task GetItamsQuantityAsync_ShouldReturnCorrectQuantity()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.CartProducts.AddRangeAsync(
                new CartProduct { ShoppingCart = cart, Quantity = 2, ProductId = 1 },
                new CartProduct { ShoppingCart = cart, Quantity = 3, ProductId = 2 }
            );
            await dbContext.SaveChangesAsync();

            var quantity = await shoppingCartService.GetItamsQuantityAsync(user.Id.ToString());

            Assert.That(5, Is.EqualTo(quantity));
        }

        [Test]
        public async Task GetItamsQuantityAsync_ShouldReturnZeroQuantity()
        {
            var userId = Guid.NewGuid().ToString();

            var quantity = await shoppingCartService.GetItamsQuantityAsync(userId);

            Assert.That(0, Is.EqualTo(quantity));
        }

        //AddToCartAsync
        [Test]
        public async Task AddToCartAsync_ShouldAddNewProductToCart()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.SaveChangesAsync();

            await shoppingCartService.AddToCartAsync(user.Id.ToString(), 1);

            var cartProduct = await dbContext.CartProducts.FirstOrDefaultAsync();

            Assert.That(1, Is.EqualTo(cartProduct.ProductId));
            Assert.That(1, Is.EqualTo(cartProduct.Quantity));
        }

        [Test]
        public async Task AddToCartAsync_ShouldIncreaseProductQuantity()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            var product = new CartProduct { ShoppingCartId = cart.Id, Quantity = 2, ProductId = 1 };
            await dbContext.CartProducts.AddAsync(product);

            await dbContext.SaveChangesAsync();

            await shoppingCartService.AddToCartAsync(user.Id.ToString(), 1);

            var cartProduct = await dbContext.CartProducts.FirstOrDefaultAsync();

            Assert.That(1, Is.EqualTo(cartProduct.ProductId));
            Assert.That(3, Is.EqualTo(cartProduct.Quantity));
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewProductWithUnExistProductId()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            var product = new CartProduct { ShoppingCartId = cart.Id, ProductId = 1 };
            await dbContext.CartProducts.AddAsync(product);

            await dbContext.SaveChangesAsync();

            await shoppingCartService.AddToCartAsync(user.Id.ToString(), 2);

            var cartProduct = await dbContext.CartProducts.Select(a => a.Quantity).ToArrayAsync();

            Assert.That(1, Is.EqualTo(cartProduct[1]));
        }

        //GetAllItemsAsync
        [Test]
        public async Task GetAllItemsAsync_ShouldReturnCorrectItems()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.Products.AddRangeAsync(
              new Product
              {
                  Id = 1,
                  Name = "Product1",
                  Price = 10,
                  Image = "image1"
              },
              new Product
              {
                  Id = 2,
                  Name = "Product2",
                  Price = 20,
                  Image = "image2"
              }
         );

            await dbContext.CartProducts.AddRangeAsync(
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 2,
                                   ProductId = 1
                               },
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 3,
                                   ProductId = 2
                               }
          );

            await dbContext.SaveChangesAsync();

            var items = await shoppingCartService.GetAllItemsAsync(user.Id.ToString());

            Assert.That(2, Is.EqualTo(items.Count()));
            Assert.That(1, Is.EqualTo(items.First().ProductId));
            Assert.That(2, Is.EqualTo(items.Last().ProductId));
        }

        [Test]
        public async Task GetAllItemsAsync_ShouldReturnEmptyItems()
        {
            var userId = Guid.NewGuid().ToString();

            var items = await shoppingCartService.GetAllItemsAsync(userId);

            Assert.That(0, Is.EqualTo(items.Count()));
        }

        //RemoveFromCartAsync
        [Test]
        public async Task RemoveFromCartAsync_ShouldDecreaseProductQuantityFromCart()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.CartProducts.AddRangeAsync(
                               new CartProduct 
                               { 
                                   ShoppingCart = cart,
                                   Quantity = 2, 
                                   ProductId = 1 
                               },
                               new CartProduct 
                               {
                                   ShoppingCart = cart,
                                   Quantity = 3,
                                   ProductId = 2 
                               }
            );
            await dbContext.SaveChangesAsync();

            await shoppingCartService.RemoveFromCartAsync(user.Id.ToString(), 1);

            var cartProducts = await dbContext.CartProducts.ToArrayAsync();

            Assert.That(1, Is.EqualTo(cartProducts[0].Quantity));
            Assert.That(1, Is.EqualTo(cartProducts[0].ProductId));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveProducyFromCart()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.CartProducts.AddAsync(
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 1,
                                   ProductId = 1
                               }
            );
            await dbContext.SaveChangesAsync();

            await shoppingCartService.RemoveFromCartAsync(user.Id.ToString(), 1);

            var cartProducts = await dbContext.CartProducts.ToArrayAsync();

            Assert.That(0, Is.EqualTo(cartProducts.Count()));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldThrowExceptionWithInvalidProductId()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.CartProducts.AddAsync(
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 1,
                                   ProductId = 1
                               }
            );
            await dbContext.SaveChangesAsync();

            Assert.That(async () => await shoppingCartService.RemoveFromCartAsync(user.Id.ToString(), 2),
                               Throws.InvalidOperationException);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldThrowExceptionWithInvalidUserId()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.CartProducts.AddAsync(
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 1,
                                   ProductId = 1
                               }
            );
            await dbContext.SaveChangesAsync();

            Assert.That(async () => await shoppingCartService.RemoveFromCartAsync(Guid.NewGuid().ToString(), 1),
                               Throws.InvalidOperationException);
        }

        //ClearCartAsync
        [Test]
        public async Task ClearCartAsync_ShouldRemoveAllProductsFromCart()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid() };
            await dbContext.AddAsync(user);

            var cart = new ShoppingCart { ApplicationUser = user };
            await dbContext.ShoppingCarts.AddAsync(cart);

            await dbContext.CartProducts.AddRangeAsync(
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 1,
                                   ProductId = 1
                               },
                               new CartProduct
                               {
                                   ShoppingCart = cart,
                                   Quantity = 2,
                                   ProductId = 2
                               }
            );
            await dbContext.SaveChangesAsync();

            await shoppingCartService.ClearCartAsync(user.Id.ToString());

            var cartProducts = await dbContext.CartProducts.ToArrayAsync();

            Assert.That(0, Is.EqualTo(cartProducts.Count()));
        }

        [Test]
        public async Task ClearCartAsync_ShouldThrowExceptionWithInvalidUserId()
        {
            try
            {
                await shoppingCartService.ClearCartAsync(Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidOperationException>());
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
