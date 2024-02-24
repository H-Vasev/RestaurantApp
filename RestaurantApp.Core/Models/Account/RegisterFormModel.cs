using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Register;
namespace RestaurantApp.Core.Models.Account
{
    public class RegisterFormModel
    {
        [Required]
        [StringLength(UsernameMaxLengh, MinimumLength = UsernameMinLengh)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(FirstnameMaxLenght, MinimumLength = FirstnameMinLenght)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(LastnameMaxLenght, MinimumLength = LastnameMinLenght)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLengh, MinimumLength = EmailMinLengh)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(CityMaxLenght, MinimumLength = CityMinLenght)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(StreetMaxLenght, MinimumLength = StreetMinLenght)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(PostalCodeMaxLengh, MinimumLength = PostalCodeMinLengh)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Password do not match!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
