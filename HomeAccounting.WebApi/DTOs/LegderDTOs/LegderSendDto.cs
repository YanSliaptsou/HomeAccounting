using HomeAccounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs
{
    public class LegderSendDto
    {
        
        public int? AccountFromId { get; set; }

        [Required(ErrorMessage = "Account id is required")]
        public int AccountToId { get; set; }

        public decimal? AmmountFrom { get; set; }

        [Required(ErrorMessage = "Account id is required")]
        public decimal AmmountTo { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public byte Type { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
