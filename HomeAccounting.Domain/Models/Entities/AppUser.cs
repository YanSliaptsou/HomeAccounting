﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public int CurrencyId { get; set; }
        public Currency MainCurrency { get; set; }
    }
}