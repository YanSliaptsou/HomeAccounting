using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.DTOs.ViewDTOs;
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
    public class ExhangeRatesRepository : IExchangeRatesRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public ExhangeRatesRepository(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
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
