using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Configurations;
using RestaurantApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.UnitTests
{
    public class GalleryServiceTests
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext dbContext;
        private IGalleryService galleryService;

        [SetUp]
        public void SetUp()
        {
            DatabaseSeedController.SeedEnabled = false;

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new ApplicationDbContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            galleryService = new GalleryService(dbContext);
        }

        //GetAllGalleryImagesAsync
        [Test]
        public async Task GetAllGalleryImagesAsync_ShouldReturnAllGalleryImages()
        {
            var galleryImages = new List<GalleryImage>()
            {
                new GalleryImage
                {
                    Id = 1,
                    Caption = "Caption",
                    CreatedOn = DateTime.Now,
                    ImagePaht = "Path",
                    ViewsCount = 0,
                    LikesCount = 0,
                },
                new GalleryImage
                {
                    Id = 2,
                    Caption = "Caption",
                    CreatedOn = DateTime.Now,
                    ImagePaht = "Path",
                    ViewsCount = 0,
                    LikesCount = 0,
                }
            };

            await dbContext.GalleryImages.AddRangeAsync(galleryImages);
            await dbContext.SaveChangesAsync();

            var result = await galleryService.GetAllGalleryImagesAsync();

            Assert.That(2, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetAllGalleryImagesAsync_ShouldReturnZeroCount()
        {
           var result = await galleryService.GetAllGalleryImagesAsync();

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        //IncrementImageViewsCountAsync
        [Test]
        public async Task IncrementImageViewsCountAsync_ShouldIncrementViewsCount()
        {
            var galleryImage = new GalleryImage
            {
                Id = 1,
                Caption = "Caption",
                CreatedOn = DateTime.Now,
                ImagePaht = "Path",
                ViewsCount = 0,
                LikesCount = 0,
            };

            await dbContext.GalleryImages.AddAsync(galleryImage);
            await dbContext.SaveChangesAsync();

            var result = await galleryService.IncrementImageViewsCountAsync(1, false);

            Assert.That(1, Is.EqualTo(result));
        }

        [Test]
        public async Task IncrementImageViewsCountAsync_ShouldReturnTowIncrementViews()
        {
            var galleryImage = new GalleryImage
            {
                Id = 1,
                Caption = "Caption",
                CreatedOn = DateTime.Now,
                ImagePaht = "Path",
                ViewsCount = 1,
                LikesCount = 0,
            };

            await dbContext.GalleryImages.AddAsync(galleryImage);
            await dbContext.SaveChangesAsync();

            var result = await galleryService.IncrementImageViewsCountAsync(1, false);

            Assert.That(2, Is.EqualTo(result));
        }

        [Test]
        public async Task IncrementImageViewsCountAsync_ShouldNotIncrementViewsCount()
        {
            var galleryImage = new GalleryImage
            {
                Id = 1,
                Caption = "Caption",
                CreatedOn = DateTime.Now,
                ImagePaht = "Path",
                ViewsCount = 0,
                LikesCount = 0,
            };

            await dbContext.GalleryImages.AddAsync(galleryImage);
            await dbContext.SaveChangesAsync();

            var result = await galleryService.IncrementImageViewsCountAsync(1, true);

            Assert.That(0, Is.EqualTo(result));
        }

        [Test]
        public async Task IncrementImageViewsCountAsync_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await galleryService.IncrementImageViewsCountAsync(1, false));
        }


        //IncrementLikeCountAsync
        [Test]
        public async Task IncrementLikeCountAsync_ShouldIncrementLikesCount()
        {
            var galleryImage = new GalleryImage
            {
                Id = 1,
                Caption = "Caption",
                CreatedOn = DateTime.Now,
                ImagePaht = "Path",
                ViewsCount = 0,
                LikesCount = 0,
                ApplicationUserId = Guid.NewGuid(),
            };

            await dbContext.GalleryImages.AddAsync(galleryImage);
            await dbContext.SaveChangesAsync();

            var userId = Guid.NewGuid().ToString();
            await galleryService.IncrementLikeCountAsync(1, userId);

            var result = await dbContext.GalleryImages.FirstOrDefaultAsync(x => x.Id == 1);

            Assert.That(1, Is.EqualTo(result.LikesCount));
        }

        [Test]
        public async Task IncrementLikeCountAsync_ShouldReturnLikesCountEquakToTwo()
        {
            var galleryImage = new GalleryImage
            {
                Id = 1,
                Caption = "Caption",
                CreatedOn = DateTime.Now,
                ImagePaht = "Path",
                ViewsCount = 0,
                LikesCount = 1,
                ApplicationUserId = Guid.NewGuid(),
            };

            await dbContext.GalleryImages.AddAsync(galleryImage);
            await dbContext.SaveChangesAsync();

            var userId = Guid.NewGuid().ToString();
            await galleryService.IncrementLikeCountAsync(1, userId);

            var result = await dbContext.GalleryImages.FirstOrDefaultAsync(x => x.Id == 1);

            Assert.That(2, Is.EqualTo(result.LikesCount));
        }

        [Test]
        public async Task IncrementLikeCountAsync_ShouldNotIncrementLikesCount()
        {
            var userId = Guid.NewGuid();
            var galleryImage = new GalleryImage
            {
                Id = 1,
                Caption = "Caption",
                CreatedOn = DateTime.Now,
                ImagePaht = "Path",
                ViewsCount = 0,
                LikesCount = 0,
                ApplicationUserId = userId,
            };

            await dbContext.GalleryImages.AddAsync(galleryImage);
            await dbContext.SaveChangesAsync();

            await galleryService.IncrementLikeCountAsync(1, userId.ToString());

            var result = await dbContext.GalleryImages.FirstOrDefaultAsync(x => x.Id == 1);

            Assert.That(0, Is.EqualTo(result.LikesCount));
        }

        [Test]
        public async Task IncrementLikeCountAsync_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await galleryService
                    .IncrementLikeCountAsync(1, Guid.NewGuid().ToString())
            );
        }

        [TearDown]
        public void TearDown()
        {
            DatabaseSeedController.SeedEnabled = true;
            dbContext.Dispose();
        }

    }
}
