using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.AccountsDTO
{
    public class AccountResponseDto
    {
        public int Id { get; set; }
        public int? TransactionCategoryId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string CurrencyId { get; set; }
    }
}
