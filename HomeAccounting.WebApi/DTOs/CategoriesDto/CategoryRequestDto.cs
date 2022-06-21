using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.CategoriesDto
{
    public class CategoryRequestDto
    {
        public int? ParentTransactionCategoryId { get; set; }
        public string Name { get; set; }
    }
}
