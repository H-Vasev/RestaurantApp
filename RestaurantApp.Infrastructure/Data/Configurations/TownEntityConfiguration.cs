using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class TownEntityConfiguration : IEntityTypeConfiguration<Town>
	{
		public void Configure(EntityTypeBuilder<Town> builder)
		{
			builder.HasData(GenerateTowns());
		}

		private Town GenerateTowns()
		{
			return new Town()
			{
				Id = 1,
				TownName = "London"
			};
		}
	}
}
