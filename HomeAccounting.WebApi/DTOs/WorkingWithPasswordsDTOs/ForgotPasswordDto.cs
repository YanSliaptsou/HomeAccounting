using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.WorkingWithPasswordsDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? ClientURI { get; set; }
    }
}
