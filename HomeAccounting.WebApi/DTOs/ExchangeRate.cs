using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs
{
    public class ExchangeRate
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal ExchangeRateValue { get; set; }
    }
}
