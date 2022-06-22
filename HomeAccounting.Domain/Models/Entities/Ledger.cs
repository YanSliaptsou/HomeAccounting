using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class Ledger
    {
        [Key]
        public int Id { get; set; }
        public Account AccountFrom { get; set; }
        public Account AccountTo { get; set; }
        public int? AccountFromId { get; set; }
        public int AccountToId { get; set; }
        public decimal? AmmountFrom { get; set; }
        public decimal AmmountTo { get; set; }
        public LedgerType Type { get; set; }
        public AppUser User { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
    }
}
