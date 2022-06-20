using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Models.Entities.Reports;
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
    public class RepItemsService : IRepItemsService
    {
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly IAccountService _accountService;
        private readonly IRepCalculatorService _repCalculatorService;

        public RepItemsService(IParentTransactionCategoryRepository parentTransactionCategoryRepository, 
                                IUserRepository userRepository, 
                                IAccountRepository accountRepository, 
                                IExchangeRatesService exchangeRatesService, 
                                IAccountService accountService, 
                                IRepCalculatorService repCalculatorService)
        {
            _parentTransactionCategoryRepository = parentTransactionCategoryRepository;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _exchangeRatesService = exchangeRatesService;
            _accountService = accountService;
            _repCalculatorService = repCalculatorService;
        }

        public async Task<AccountReport> GetAccountReport(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            var user = await _userRepository.GetConcreteUser(account.AppUserId);
            var sumInLocalCurrency = await _repCalculatorService.CalculateByAccount(accountId, dateFrom, dateTo);
            var percentage = await _repCalculatorService.CalculatePercentageByAccount(accountId, dateFrom, dateTo);
            var exchangeRate = await _exchangeRatesService.GetExchangeRate(account.CurrencyId, user.MainCurrencyId);

            AccountReport incomeAccountReport = new AccountReport
            {
                Percentage = Math.Round(percentage, 2),
                SumInLocalCurrency = Math.Round(sumInLocalCurrency, 2),
                AccountName = account.Name,
                LocalCurrencyCode = account.CurrencyId,
                UsersCurrencyCode = user.MainCurrencyId,
                SumInUsersCurrency = Math.Round(sumInLocalCurrency * exchangeRate, 2)
            };

            return incomeAccountReport;
        }

        public async Task<OutcomeCategoryReport> GetOutcomeCategoryReport(int categoryId, DateTime dateFrom, DateTime dateTo)
        {
            List<Account> accounts = _accountService.GetAccountsListByCategory(categoryId).Result;

            List<AccountReport> outcomeAccountReports = await GetAccountReportListByAccounts(accounts, dateFrom, dateTo);

            var percentage = await _repCalculatorService.CalculatePercentageByCategory(categoryId, dateFrom, dateTo);
            var totalSum = await _repCalculatorService.CalculateByCategory(categoryId, dateFrom, dateTo);
            var category = await _parentTransactionCategoryRepository.GetParentCategory(categoryId);
            var user = await _userRepository.GetConcreteUser(category.UserId);

            OutcomeCategoryReport outcomeCategoryReport = new OutcomeCategoryReport
            {
                Percentage = Math.Round(percentage, 2),
                TotalSum = Math.Round(totalSum, 2),
                OutcomeAccountsReports = outcomeAccountReports,
                CategoryName = category.Name,
                Currency = user.MainCurrencyId
            };

            return outcomeCategoryReport;
        }

        public async Task<List<AccountReport>> GetAccountReportListByAccounts(IEnumerable<Account> accounts, DateTime dateFrom, DateTime dateTo)
        {
            List<AccountReport> accountsReport = new List<AccountReport>();
            foreach (var account in accounts)
            {
                if (account != null)
                {
                    accountsReport.Add(await GetAccountReport(account.Id, dateFrom, dateTo));
                }
            }
            return accountsReport;
        }

        public async Task<List<OutcomeCategoryReport>> GetCategoryReportListByCategory(IEnumerable<ParentTransactionCategory> categories, DateTime dateFrom, DateTime dateTo)
        {
            List<OutcomeCategoryReport> outcomeCategoryReports = new List<OutcomeCategoryReport>();
            foreach (var category in categories)
            {
                outcomeCategoryReports.Add(await GetOutcomeCategoryReport(category.Id, dateFrom, dateTo));
            }

            return outcomeCategoryReports;
        }
    }
}
