using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			if (DatabaseSeedController.SeedEnabled)
			{
                builder.HasData(GenerateCategories());
            }
        }

		private Category[] GenerateCategories()
		{
			var categories = new HashSet<Category>();

			Category category;

			category = new Category
			{
				Id = 1,
				CategoryName = "Starters"
			};
			categories.Add(category);

			category = new Category
			{
				Id = 2,
				CategoryName = "Salads"
			};
			categories.Add(category);

			category = new Category
			{
				Id = 3,
				CategoryName = "Specialty"
			};
			categories.Add(category);

			return categories.ToArray();
		}
	}
}
