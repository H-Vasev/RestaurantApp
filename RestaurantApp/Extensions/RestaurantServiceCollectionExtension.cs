using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using NuGet.Protocol;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RestaurantServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IChatService, ChatService>();

            var googleCredentials = configuration;
            var filePath = @"C:\Users\44770\AppData\Roaming\Microsoft\UserSecrets\google-storage-key\restaurantapp-420520-df296ef87671.json";
            var text = File.ReadAllText(filePath);
            var credentials = GoogleCredential.FromFile(filePath);
            services.AddSingleton(s => StorageClient.Create(credentials));

            return services;
        }
    }
}
