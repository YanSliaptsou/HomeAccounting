using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.RegistrationDTOs
{
    public class UserRegistrationRequestDTO
    {
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? MainCurrencyId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public string? ClientURI { get; set; }
    }
}
