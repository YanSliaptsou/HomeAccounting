using HomeAccounting.Domain.DTOs.ViewDTOs;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Abstarct
{
    public interface IExchangeRatesRepository
    {
        Task<IEnumerable<ExchangeRate>> GetAllExchaneRates();
        Task<IEnumerable<ExchangeRate>> GetConcreteCurrencyExchangeRate(string currencyCode);
    }
}
