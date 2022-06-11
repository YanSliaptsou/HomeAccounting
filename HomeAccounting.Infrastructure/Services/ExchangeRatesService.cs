using HomeAccounting.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeAccounting.Domain.Repositories.Interfaces;

namespace HomeAccounting.Infrastructure.Services.Concrete
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly IExchangeRatesRepository _exchangeRatesRepository;
        public ExchangeRatesService(IExchangeRatesRepository exchangeRatesRepository)
        {
            _exchangeRatesRepository = exchangeRatesRepository;
        }

        public async Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo)
        {
            decimal rate = 0;
            var rates = await _exchangeRatesRepository.GetAllExchaneRates();
            currencyFrom = currencyFrom.ToUpper();
            currencyTo = currencyTo.ToUpper();

            var exchangeRate = rates.FirstOrDefault(x => x.CurrencyFromId == currencyFrom && x.CurrencyToId == currencyTo);
            if (exchangeRate != null)
            {
                rate = exchangeRate.AmountTo / exchangeRate.AmountFrom;
            }
            else
            {
                exchangeRate = rates.FirstOrDefault(x => x.CurrencyFromId == currencyTo && x.CurrencyToId == currencyFrom);
                if (exchangeRate != null)
                {
                    rate = exchangeRate.AmountFrom / exchangeRate.AmountTo;
                }
            }

            return rate;
        }
    }
}
