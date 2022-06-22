using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models.Entities.Reports
{
    public class AccountReport
    {
        public string AccountName { get; set; }
        public decimal SumInLocalCurrency { get; set; }
        public string LocalCurrencyCode { get; set; }
        public decimal SumInUsersCurrency { get; set; }
        public string UsersCurrencyCode { get; set; }
        public decimal Percentage { get; set; }
    }
}
