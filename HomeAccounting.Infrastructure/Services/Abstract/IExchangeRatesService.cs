using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface IExchangeRatesService
    {
        Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo);
    }
}
