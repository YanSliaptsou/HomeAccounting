using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public int? TransactionCategoryId { get; set; }
        public string Name { get; set; }
        public Currency Сurrency { get; set; }
        public string CurrencyId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
