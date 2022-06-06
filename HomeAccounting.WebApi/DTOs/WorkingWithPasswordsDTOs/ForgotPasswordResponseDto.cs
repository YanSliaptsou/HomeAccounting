using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.WorkingWithPasswordsDTOs
{
    public class ForgotPasswordResponseDto
    {
        public bool IsPawwordReseted { get; set; }
        public string ResetToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
