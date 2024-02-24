using RestaurantApp.Core.Models.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Contracts
{
    public interface ITownService
    {
        Task<TownModel?> GetTownByNameAsync(string city);
    }
}
