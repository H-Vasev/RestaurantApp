using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Menu;

namespace RestaurantApp.Areas.Administrator.Controllers
{
	public class ProductController : BaseAdministratorController
	{
		private readonly IMenuService menuService;
		private readonly IProductService productService;
		private readonly StorageClient storageClient;
		private readonly string bucketName = "bucketstorage123";

		public ProductController(IMenuService menuService, IProductService productService, StorageClient storageClient)
		{
			this.menuService = menuService;
			this.productService = productService;
			this.storageClient = storageClient;
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
			var maxFileSize = 2000000;
			if (imagePath != null && imagePath.Length > 0 && imagePath.Length < maxFileSize)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagePath.FileName);
				var contentType = imagePath.ContentType;

				if (contentType == "image/png" || contentType == "image/jpeg")
				{
					try
					{
						var imageObject = await storageClient.UploadObjectAsync(bucketName, fileName, contentType, imagePath.OpenReadStream());
						model.ImagePath = imageObject.MediaLink;
					}
					catch (Exception ex)
					{
						TempData["Error"] = "Failed to upload image: " + ex.Message;
						ModelState.AddModelError("", "Failed to upload image: " + ex.Message);
						return View(model);
					}
				}
			}
			else
			{
				TempData["Error"] = "Invalid image format. Only PNG and JPEG are allowed.";
				ModelState.AddModelError("", "Invalid image format. Only PNG and JPEG are allowed.");
				return View(model);
			}

			try
			{
				await productService.AddProductAsync(model);
			}
			catch (Exception)
			{

				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var model = await productService.GetProductByIdForEditAsync(id);
				model.Categories = await menuService.GetCategoriesAsync();

				return View(model);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ProductFormModel model, int id)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = await menuService.GetCategoriesAsync();
				return View(model);
			}

			try
			{
				await productService.EditProductAsync(model, id);
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> Remove(int id)
		{
			var imgPath = await productService.GetProductImagePathAsync(id);

			if (!string.IsNullOrEmpty(imgPath))
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", imgPath);

				if (filePath != null)
				{
					try
					{
						if (System.IO.File.Exists(filePath))
						{
							System.IO.File.Delete(filePath);
						}
						else
						{
							var uri = new Uri(imgPath);
							var imgName = System.Web.HttpUtility.UrlDecode(Path.GetFileName(uri.LocalPath));

							storageClient.DeleteObject(bucketName, imgName);
						}
					}
					catch (Exception)
					{
						return RedirectToAction("Error", "Home");
					}
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
				return RedirectToAction("Error", "Home");
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
