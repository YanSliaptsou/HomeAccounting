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
        public Currency CurrencyFrom { get; set; }
        public Currency CurrencyTo { get; set; }
        public string? CurrencyFromId { get; set; }
        public string CurrencyToId { get; set; }
        public decimal? AmmountFrom { get; set; }
        public decimal AmmountTo { get; set; }
        public string Type { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
    }
}
