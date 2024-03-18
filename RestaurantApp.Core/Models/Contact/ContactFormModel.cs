using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Core.Constants.ModelsValidations.ContactFormModel;

namespace RestaurantApp.Core.Models.Contact
{
	public class ContactFormModel
	{
		[Required]
		[StringLength(NameMaxLenght, MinimumLength = NameMinLenght)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(EmailMaxLenght, MinimumLength = EmailMinLenght)]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[StringLength(SubjectMaxLenght, MinimumLength = SubjectMinLenght)]
		public string Subject { get; set; } = null!;

		[Required]
		[StringLength(MessageMaxLenght, MinimumLength = MessageMinLenght)]
		public string Message { get; set; } = null!;
	}
}
