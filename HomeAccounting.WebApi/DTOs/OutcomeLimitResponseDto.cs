using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs
{
    public class OutcomeLimitResponseDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Limit { get; set; }
        public DateTime? LimitFrom { get; set; }
        public DateTime? LimitTo { get; set; }
        public decimal TotalSpend { get; set; }
        public decimal Percentage { get; set; }
    }
}
