﻿using RestaurantApp.Core.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Contracts
{
	public interface IMenuService
	{
        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        Task<IEnumerable<MenuViewModel>> GetMenuAsync(string? category);
	}
}
