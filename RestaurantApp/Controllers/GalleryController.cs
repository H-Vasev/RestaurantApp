using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly IGalleryService galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            this.galleryService = galleryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await galleryService.GetAllGalleryImagesAsync();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncrementImageCount(int id)
        {
            var cookieName = "IncrementedImages";
            var incrementedImages = new List<int>();
            var isIdExist = true;

            if (Request.Cookies.ContainsKey(cookieName))
            {
                var info = Request.Cookies[cookieName];
				incrementedImages = Request.Cookies[cookieName].Split(',').Select(int.Parse).ToList();
			}

            if (!incrementedImages.Contains(id))
            {
				incrementedImages.Add(id);
                var cookieOptions = new CookieOptions 
                { 
                    Expires = DateTimeOffset.Now.AddDays(30), 
                    HttpOnly = true 
                };
				Response.Cookies.Append(cookieName, string.Join(',', incrementedImages), cookieOptions);
                isIdExist = false;
			}

            var imageCount = await galleryService.IncrementImageViewsCountAsync(id, isIdExist);

            return Json(new {viewCount = imageCount});
        }
    }
}
