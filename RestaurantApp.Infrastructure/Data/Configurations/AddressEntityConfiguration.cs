using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
	{
		public void Configure(EntityTypeBuilder<Address> builder)
		{
			builder.HasData(GenerateAddresses());
		}

		private static Address GenerateAddresses()
		{
			return new Address()
			{
				Id = Guid.Parse("553FB0ED-0384-4AEB-F80D-08DC21D76B67"),
				Street = "Baker Street",
				PostalCode = "NB12 JAT",
				TownId = 1
			};
		}
	}
}
