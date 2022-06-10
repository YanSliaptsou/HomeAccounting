using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.ParentCategoriesDTO
{
    public class CreateParentCategoryDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
