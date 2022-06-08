using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs
{
    public class TransactionCategoryViewDto
    {
        public int Id { get; set; }
        public string? ParentTransactionCategoryName { get; set; }
        public string Name { get; set; }
    }
}
