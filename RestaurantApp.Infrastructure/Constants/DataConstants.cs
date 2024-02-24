namespace RestaurantApp.Infrastructure.Constants
{
	public static class DataConstants
	{
		public static class User
		{
			public const int FirstNameMaxLenght = 50;
			public const int FirstNameMinLenght = 2;

			public const int LastNameMaxLenght = 50;
			public const int LastNameMinLenght = 2;
		}

		public static class Address
		{
			public const int StreetMaxLenght = 100;
			public const int StreetMinLenght = 2;

			public const int PostalCodeMaxLenght = 50;
			public const int PostalCodeMinLenght = 2;
		}

		public static class Town
		{
			public const int TownNameMaxLenght = 50;
			public const int TownNameMinLenght = 2;
		}

		public static class Product
		{
			public const int NameMaxLenght = 50;
			public const int NameMinLenght = 2;

			public const int DescriptionMaxLenght = 500;
			public const int DescriptionMinLenght = 5;
		}

		public static class Category
		{
			public const int NameMaxLenght = 50;
			public const int NameMinLenght = 2;
		}
	}
}
