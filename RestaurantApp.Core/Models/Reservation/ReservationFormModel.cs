using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Reservation;

namespace RestaurantApp.Core.Models.Reservation
{
    public class ReservationFormModel
    {
        [Required]
        [StringLength(FirstNameMaxLenght, MinimumLength = FirstNameMinLenght)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(LastNameMaxLenght, MinimumLength = LastNameMinLenght)]
        public string LastName { get; set; } = string.Empty;

        public string? EventName { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLenght, MinimumLength = PhoneNumberMinLenght)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required]
        [StringLength(EmailMaxLenght, MinimumLength = EmailMinLenght)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Required]
        [Range(1, 12)]
        [Display(Name = "Number of people")]
        public int PeopleCount { get; set; } = 1;

        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght)]
        public string? Description { get; set; }

        public Guid? ApplicationUserId { get; set; }

        public int? EventId { get; set; }
    }
}
