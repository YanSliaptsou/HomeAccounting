using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Abstarct;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Concrete
{
    public class CurrenciesRepository : ICurrenciesRepository
    {
        private readonly DatabaseContext _context;

        public CurrenciesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            return await _context.Currencies.ToListAsync();
        }
    }
}
