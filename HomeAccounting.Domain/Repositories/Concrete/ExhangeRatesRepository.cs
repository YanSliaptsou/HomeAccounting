﻿using AutoMapper;
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

        public async Task<IEnumerable<ExchangeRatesViewDTO>> GetAllExchaneRates()
        {
            return await _databaseContext.ExchangeRates.ProjectTo<ExchangeRatesViewDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<ExchangeRatesViewDTO>> GetConcreteCurrencyExchangeRate(string currencyCode)
        {
            return await _databaseContext.ExchangeRates.Where(x => x.CurrencyFrom.Code == currencyCode || 
                    x.CurrencyTo.Code == currencyCode).ProjectTo<ExchangeRatesViewDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}