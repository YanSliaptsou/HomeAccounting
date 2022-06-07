using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface IExchangeRatesRepository
    {
        Task<IEnumerable<ExchangeRate>> GetAllExchaneRates();
        Task<IEnumerable<ExchangeRate>> GetConcreteCurrencyExchangeRate(string currencyCode);
    }
}
