using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.ParentCategoriesDTO
{
    public class ParentCategoryRequestDto
    {
        [Required(ErrorMessage = "The category name is required")]
        public string Name { get; set; }
    }
}
