using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Extensions;
using RestaurantApp.Hubs;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
			{
                options.SignIn.RequireConfirmedAccount =
                    builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                options.Password.RequireNonAlphanumeric =
                     builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                options.Password.RequireLowercase =
                    builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                options.Password.RequireUppercase =
                    builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                options.Password.RequiredLength =
                    builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            })
				.AddRoles<IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services
				.AddControllersWithViews()
				.AddMvcOptions(otions =>
				{
					otions.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
				});

			builder.Services.AddApplicationServices();
			builder.Services.AddSignalR();

			builder.Services.AddMemoryCache();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error?statusCode=500");
				app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			if (app.Environment.IsDevelopment())
			{
				app.PrepareData();
			}

			app.UseEndpoints(config =>
			{
				config.MapControllerRoute(
					name: "areas",
					pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

				config.MapControllerRoute(
					name: "default",
					pattern: "/{controller=Home}/{action=Index}/{id?}");

				config.MapHub<ChatHub>("/chatHub");

				app.MapRazorPages();
			});

			app.Run();
		}
	}
}