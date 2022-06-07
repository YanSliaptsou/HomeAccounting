using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories
{
    public class ExhangeRatesRepository : IExchangeRatesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ExhangeRatesRepository(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllExchaneRates()
        {
            return await _databaseContext.ExchangeRates.ToListAsync();
        }

        public async Task<IEnumerable<ExchangeRate>> GetConcreteCurrencyExchangeRate(string currencyCode)
        {
            return await _databaseContext.ExchangeRates.Where(x => x.CurrencyFrom.Id == currencyCode || 
                    x.CurrencyTo.Id == currencyCode).ToListAsync();
        }
    }
}
