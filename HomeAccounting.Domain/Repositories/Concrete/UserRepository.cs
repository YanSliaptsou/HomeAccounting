﻿using HomeAccounting.Domain.Repositories.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Db;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Domain.Repositories.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task ChangeMainCurrency(string userId, string currencyCode)
        {
             AppUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
             Currency currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Code == currencyCode);

             user.CurrencyId = currency.Code;
        }
    }
}