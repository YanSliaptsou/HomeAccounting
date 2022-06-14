using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models.Entities.Reports
{
    public class OutcomeCategoryReport
    {
        public string CategoryName { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
        public decimal Percentage { get; set; }
        public List<AccountReport> OutcomeAccountsReports { get; set; }
    }
}
