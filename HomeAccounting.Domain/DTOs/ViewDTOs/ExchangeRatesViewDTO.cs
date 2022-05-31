using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.DTOs.ViewDTOs
{
    public class ExchangeRatesViewDTO
    {
        public int Id { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
    }
}
