using HomeAccounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models.Entities
{
    public class OutcomeLimit
    {
        [Key]
        public int Id { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public decimal Limit { get; set; }
        public DateTime? LimitFrom { get; set; }
        public DateTime? LimitTo { get; set; }
    }
}
