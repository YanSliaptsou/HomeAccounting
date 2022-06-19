using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class HtmlSenderTemplate
    {
        [Key]
        public string Name { get; set; }
        public string Template { get; set; }
    }
}
