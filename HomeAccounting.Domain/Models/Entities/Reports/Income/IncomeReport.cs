using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models.Entities.Reports
{
    public class IncomeReport
    {
        public List<AccountReport> IncomeAccountReports { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
    }
}
