using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.ReportDto.Outcome
{
    public class OutcomeReportDto
    {
        public List<OutcomeCategoryReportDto> CategoriesReport { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
    }
}
