using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class RepCalculatorService : IRepCalculatorService
    {
        private readonly ILegderRepository _legderRepository;
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly IAccountService _accountService;

        public RepCalculatorService(ILegderRepository legderRepository, 
                                    IParentTransactionCategoryRepository parentTransactionCategoryRepository,
                                    IUserRepository userRepository,
                                    IExchangeRatesService exchangeRatesService,
                                    IAccountService accountService,
                                    IAccountRepository accountRepository)
        {
            _legderRepository = legderRepository;
            _parentTransactionCategoryRepository = parentTransactionCategoryRepository;
            _userRepository = userRepository;
            _exchangeRatesService = exchangeRatesService;
            _accountService = accountService;
            _accountRepository = accountRepository;
        }

        public async Task<decimal> CalculateByAccount(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            return _legderRepository.GetAllLegdersByAccountTo(accountId).Result.Where(x => x.DateTime >= dateFrom && x.DateTime <= dateTo).Sum(x => x.AmmountTo);
        }

        public async Task<decimal> CalculateByCategory(int categoryId, DateTime dateFrom, DateTime dateTo)
        {
            decimal totalSum = 0;

            var userId = _parentTransactionCategoryRepository.GetParentCategory(categoryId).Result.UserId;
            var userCurrency = _userRepository.GetConcreteUser(userId).Result.MainCurrencyId;
            var accountsList = _accountService.GetAccountsListByCategory(categoryId).Result;

            foreach(var account in accountsList)
            {
                totalSum += await CalculateByAccount(account.Id, dateFrom, dateTo) * 
                    await _exchangeRatesService.GetExchangeRate(account.CurrencyId, userCurrency);
            }

            return totalSum;
        }

        public async Task<decimal> CalculatePercentageByAccount(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            var userId = _accountRepository.GetAccountById(accountId).Result.AppUserId;
            var userCurrency = _userRepository.GetConcreteUser(userId).Result.MainCurrencyId;

            var accountCurrency = _accountRepository.GetAccountById(accountId).Result.CurrencyId;
            var ledgerType = await _accountService.DefineAccountType(accountId);

            var sumByAccount = await CalculateByAccount(accountId, dateFrom, dateTo);
            var totalSum = await CalculateTotal(userId, ledgerType, dateFrom, dateTo);

            return (sumByAccount * await _exchangeRatesService.GetExchangeRate(accountCurrency, userCurrency) / totalSum) * 100;
        }

        public async Task<decimal> CalculatePercentageByCategory(int categoryId, DateTime dateFrom, DateTime dateTo)
        {
            var userId = _parentTransactionCategoryRepository.GetParentCategory(categoryId).Result.UserId;
            var totalByCategory = await CalculateByCategory(categoryId, dateFrom, dateTo);
            var totalSum = await CalculateTotal(userId, LedgerType.Credit, dateFrom, dateTo);

            return (totalByCategory / totalSum) * 100;
        }

        public async Task<decimal> CalculateTotal(string userId, LedgerType type, DateTime dateFrom, DateTime dateTo)
        {
            decimal sum = 0;

            var userCurrency = _userRepository.GetConcreteUser(userId).Result.MainCurrencyId;

            var ledgersList = _legderRepository.GetAllLegdersByType(type, userId).Result.Where(x => x.DateTime >= dateFrom && x.DateTime <= dateTo);

            foreach(var ledger in ledgersList)
            {
                var accountCurrency = _accountRepository.GetAccountById(ledger.AccountToId).Result.CurrencyId;
                sum += ledger.AmmountTo * _exchangeRatesService.GetExchangeRate(accountCurrency, userCurrency).Result;
            }

            return sum;
        }
    }
}
