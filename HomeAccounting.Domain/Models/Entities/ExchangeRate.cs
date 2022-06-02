using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }
        public Currency CurrencyFrom { get; set; }
        public Currency CurrencyTo { get; set; }
        public string CurrencyFromId { get; set; }
        public string CurrencyToId { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
    }
}
