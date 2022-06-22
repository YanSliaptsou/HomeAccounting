using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models.Entities.Reports
{
    public class OutcomeReport
    {
        public List<OutcomeCategoryReport> CategoriesReport { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
    }
}
