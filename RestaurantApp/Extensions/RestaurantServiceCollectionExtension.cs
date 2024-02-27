﻿using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RestaurantServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IEventService, EventService>();

			return services;
        }
    }
}
