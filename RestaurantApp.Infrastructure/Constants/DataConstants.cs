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

			//check here
			public const int PostalCodeMaxLenght = 10;
			public const int PostalCodeMinLenght = 4;
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

        public static class Register
        {
			public const int UsernameMaxLengh = 30;
			public const int UsernameMinLengh = 4;

            public const int FirstnameMaxLenght = 50;
			public const int FirstnameMinLenght = 2;

			public const int LastnameMaxLenght = 50;
			public const int LastnameMinLenght = 2;

			public const int EmailMaxLengh = 50;
			public const int EmailMinLengh = 5;

			public const int CityMaxLenght = 50;
			public const int CityMinLenght = 2;

			public const int StreetMaxLenght = 100;
			public const int StreetMinLenght = 2;

            public const int PostalCodeMaxLengh = 20;
			public const int PostalCodeMinLengh = 4;
        }

		public static class Reservation
		{
			public const int FirstNameMaxLenght = 50;
			public const int FirstNameMinLenght = 2;

			public const int LastNameMaxLenght = 50;
			public const int LastNameMinLenght = 2;

			public const int PhoneNumberMaxLenght = 25;
			public const int PhoneNumberMinLenght = 8;

			public const int EmailMaxLenght = 30;
			public const int EmailMinLenght = 5;

			public const int DescriptionMaxLenght = 200;
			public const int DescriptionMinLenght = 5;

			public const int PeopleCountMax = 60;
			public const int PeopleCountMin = 1;
		}

		public static class Event
		{
			public const int TitleMaxLenght = 50;
			public const int TitleMinLenght = 2;

			public const int DescriptionMaxLenght = 50;
			public const int DescriptionMinLenght = 2;
		}

		public static class GalleryImage
		{
			public const int CaptionMaxLenght = 50;
			public const int CaptionMinLenght = 2;
		}

		public static class Chat
		{
			public const int UsernameMaxLenght = 30;
			public const int UsernameMinLenght = 3;
		}

		public static class ChatMessage
		{
			public const int MessageMaxLenght = 500;
			public const int MessageMinLenght = 1;

			public const int SenderNameMaxLenght = 30;
			public const int SenderNameMinLenght = 3;
		}
    }
}
