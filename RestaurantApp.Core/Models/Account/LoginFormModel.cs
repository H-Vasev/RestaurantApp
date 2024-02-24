using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Register;

namespace RestaurantApp.Core.Models.Account
{
    public class LoginFormModel
    {
        [Required]
        [StringLength(UsernameMaxLengh, MinimumLength = UsernameMinLengh)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
