﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.DTOs.ViewDTOs
{
    public class TransactionCategoryViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Constraint { get; set; }
        public double? ParentCategoryName { get; set; }
    }
}