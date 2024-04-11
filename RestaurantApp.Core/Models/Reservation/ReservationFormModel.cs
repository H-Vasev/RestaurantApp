using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Reservation;
using static RestaurantApp.Core.Constants.MessageConstants;

namespace RestaurantApp.Core.Models.Reservation
{
    public class ReservationFormModel
    {
        [Required(ErrorMessage = RequiredField)]
        [Display(Name = "First Name")]
        [StringLength(FirstNameMaxLenght, MinimumLength = FirstNameMinLenght)]
        public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredField)]
        [Display(Name = "Last Name")]
		[StringLength(LastNameMaxLenght, MinimumLength = LastNameMinLenght)]
        public string LastName { get; set; } = string.Empty;

        public string? EventName { get; set; }

		[Required(ErrorMessage = RequiredField)]
        [Display(Name = "Phone Number")]
		[StringLength(PhoneNumberMaxLenght, MinimumLength = PhoneNumberMinLenght)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;


		[Required(ErrorMessage = RequiredField)]
		[StringLength(EmailMaxLenght, MinimumLength = EmailMinLenght)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredField)]
		public string Date { get; set; } = string.Empty;

		[Required]
        [Display(Name = "Number of peaople")]
        [Range(PeopleCountMin, PeopleCountMax, ErrorMessage = FieldLength)]
        public int PeopleCount { get; set; } = 1;

        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght)]
        public string? Description { get; set; }

        public Guid? ApplicationUserId { get; set; }

        public int? EventId { get; set; }
    }
}
