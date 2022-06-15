using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.ReportDto.Income
{
    public class IncomeReportDto
    {
        public List<AccountReportDto> IncomeAccountReports { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
    }
}
