using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class Ledger
    {
        [Key]
        public int Id { get; set; }
        public TransactionCategory CategoryFrom { get; set; }
        public TransactionCategory CategoryTo { get; set; }
        public Currency CurrencyFrom { get; set; }
        public Currency CurrencyTo { get; set; }
        public string Type { get; set; }
        public AppUser User { get; set; }
    }
}
