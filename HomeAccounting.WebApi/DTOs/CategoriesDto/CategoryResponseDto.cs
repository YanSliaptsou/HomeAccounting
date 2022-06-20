using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.CategoriesDto
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public int? ParentTransactionCategoryId { get; set; }
        public string Name { get; set; }
    }
}
