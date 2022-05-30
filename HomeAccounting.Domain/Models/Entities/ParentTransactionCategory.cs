using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class ParentTransactionCategory
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public AppUser User { get; set; }
    }
}
