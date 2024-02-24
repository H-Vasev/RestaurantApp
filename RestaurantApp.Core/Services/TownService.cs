using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Town;
using RestaurantApp.Data;

namespace RestaurantApp.Core.Services
{
    public class TownService : ITownService
    {
        private readonly ApplicationDbContext dbContext;

        public TownService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<TownModel?> GetTownByNameAsync(string city)
        {
            return await dbContext.Towns
                .Where(t => t.TownName == city)
                .Select(t => new TownModel
                {
                    Id = t.Id,
                    Name = t.TownName
                })
                .FirstOrDefaultAsync();
        }
    }
}
