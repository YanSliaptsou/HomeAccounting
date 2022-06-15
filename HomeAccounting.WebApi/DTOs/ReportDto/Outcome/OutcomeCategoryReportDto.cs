using HomeAccounting.Domain.Models.Entities.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.ReportDto.Outcome
{
    public class OutcomeCategoryReportDto
    {
        public string CategoryName { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
        public decimal Percentage { get; set; }
        public List<AccountReport> OutcomeAccountsReports { get; set; }
    }
}
