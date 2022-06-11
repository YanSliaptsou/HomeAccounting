using HomeAccounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs
{
    public class LedgerResponseDto
    {
        public int Id { get; set; }
        public string AccountNameFrom { get; set; }
        public string AccountNameTo { get; set; }
        public decimal? AmmountFrom { get; set; }
        public decimal AmmountTo { get; set; }
        public LedgerType Type { get; set; }
        public DateTime DateTime { get; set; }
    }
}
