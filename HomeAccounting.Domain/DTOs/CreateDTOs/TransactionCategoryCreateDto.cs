using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.DTOs.CreateDTOs
{
    public class TransactionCategoryCreateDto
    {
        public string Name { get; set; }
        public decimal Constraint { get; set; }
        public int? ParentTransactionCategoryId { get; set; }
    }
}
