using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class TransactionCategory
    {
        [Key]
        public int Id { get; set; }
        public int? ParentTransactionCategoryId { get; set; }
        public ParentTransactionCategory ParentTransactionCategory { get; set; }
        public string Name { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
    }
}
