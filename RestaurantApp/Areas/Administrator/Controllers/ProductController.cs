using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Menu;

namespace RestaurantApp.Areas.Administrator.Controllers
{
    public class ProductController : BaseAdministratorController
	{
		private readonly IMenuService menuService;
		private readonly IProductService productService;

        public ProductController(IMenuService menuService, IProductService productService)
        {
			this.menuService = menuService;
			this.productService = productService;
        }

        public async Task<IActionResult> Index(string? category)
		{
			var model = await menuService.GetMenuAsync(category);

			ViewBag.Categories = await menuService.GetCategoriesAsync();
			ViewBag.CurrentCategory = category;

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var model = new ProductFormModel();
			model.Categories = await menuService.GetCategoriesAsync();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProductFormModel model, IFormFile imagePath)
		{
            if (!ModelState.IsValid)
			{
                model.Categories = await menuService.GetCategoriesAsync();
                return View(model);
            }

			if (imagePath != null && imagePath.Length > 0)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagePath.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/menu", fileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await imagePath.CopyToAsync(fileStream);
				}

				model.ImagePath = "img/menu/" + fileName;
			}

			await productService.AddProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
		public async Task<IActionResult> Remove(int id)
		{
			var imgPath = await productService.GetProductImagePathAsync(id);

			if (!string.IsNullOrEmpty(imgPath))
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", imgPath);

				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				else
				{
					return NotFound("Image not found.");
				}
			}
			else
			{
				return NotFound("Image not found.");
			}

			try
			{
                await productService.RemoveProductAsync(id);
            }
            catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
