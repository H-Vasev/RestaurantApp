using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Constants
{
	public static class ModelsValidations
	{
		public static class CheckoutFormModel
		{
			public const int NameMaxLenght = 50;
			public const int NameMinLenght = 3;

			public const int AddressMaxLenght = 100;
			public const int AddressMinLenght = 3;

			public const int PostalCodeMaxLenght = 10;
			public const int PostalCodeMinLenght = 4;

			public const int EmailMaxLenght = 20;
			public const int EmailMinLenght = 4;
		}

		public static class ContactFormModel
		{
			public const int NameMaxLenght = 50;
			public const int NameMinLenght = 3;

			public const int EmailMaxLenght = 50;
			public const int EmailMinLenght = 4;

			public const int SubjectMaxLenght = 50;
			public const int SubjectMinLenght = 3;

			public const int MessageMaxLenght = 500;
			public const int MessageMinLenght = 5;
		}
	}
}
