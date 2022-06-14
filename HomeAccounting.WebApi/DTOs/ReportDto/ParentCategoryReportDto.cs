using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.ReportDto
{
    public class ParentCategoryReportDto
    {
        public string CategoryName { get; set; }
        public decimal TotalSum { get; set; }
        public string Currency { get; set; }
        public decimal Percentage { get; set; }
        public List<CategoryReportDto> SubcategoryReports { get; set; }
    }
}
