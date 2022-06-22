using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.AccountsDTO
{
    public class AccountRequestDto
    {
        public int? TransactionCategoryId { get; set; }

        [Required(ErrorMessage ="Account name is required")]
        public string Name { get; set; }

        public string Type { get; set; }

        [Required(ErrorMessage = "Account currency is required")]
        public string CurrencyId { get; set; }
    }
}
