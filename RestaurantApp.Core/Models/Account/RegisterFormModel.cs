using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Register;
using static RestaurantApp.Core.Constants.MessageConstants;

namespace RestaurantApp.Core.Models.Account
{
	public class RegisterFormModel
    {
        [Required]
        [StringLength(UsernameMaxLengh, MinimumLength = UsernameMinLengh)]
        public string Username { get; set; } = string.Empty;

		[Display(Name = "First Name")]
		[Required(ErrorMessage = RequiredField)]
        [StringLength(FirstnameMaxLenght, MinimumLength = FirstnameMinLenght)]
        public string FirstName { get; set; } = string.Empty;

		[Display(Name = "Last Name")]
		[Required(ErrorMessage = RequiredField)]
		[StringLength(LastnameMaxLenght, MinimumLength = LastnameMinLenght)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredField)]
        [EmailAddress]
        [StringLength(EmailMaxLengh, MinimumLength = EmailMinLengh)]
        public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredField)]
		[StringLength(CityMaxLenght, MinimumLength = CityMinLenght)]
        public string City { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredField)]
		[StringLength(StreetMaxLenght, MinimumLength = StreetMinLenght)]
        public string Street { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredField)]
		[StringLength(PostalCodeMaxLengh, MinimumLength = PostalCodeMinLengh)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password do not match!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
