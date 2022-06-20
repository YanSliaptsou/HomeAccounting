using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Models.Entities.Reports;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class RepConstructorService : IRepConstructorService
    {
        private readonly IRepCalculatorService _repCalculatorService;
        private readonly IRepItemsService _repItemsService;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;

        public RepConstructorService(IRepCalculatorService repCalculatorService, IRepItemsService repItemsService, IAccountRepository accountRepository, IUserRepository userRepository, IParentTransactionCategoryRepository parentTransactionCategoryRepository)
        {
            _repCalculatorService = repCalculatorService;
            _repItemsService = repItemsService;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _parentTransactionCategoryRepository = parentTransactionCategoryRepository;
        }

        public async Task<IncomeReport> GetFullIncomeReport(string userId, DateTime dateFrom, DateTime dateTo)
        {
            var accounts = _accountRepository.GetAllAcountsByType(userId, "Income").Result;
            var user = await _userRepository.GetConcreteUser(userId);
            var totalSum = await _repCalculatorService.CalculateTotal(userId, Domain.Enums.LedgerType.Debet, dateFrom, dateTo);

            List<AccountReport> incomeAccountReports = await _repItemsService.GetAccountReportListByAccounts(accounts, dateFrom, dateTo);

            IncomeReport incomeReport = new IncomeReport
            {
                IncomeAccountReports = incomeAccountReports,
                Currency = user.MainCurrencyId,
                TotalSum = Math.Round(totalSum, 2)
            };

            return incomeReport;
        }

        public async Task<OutcomeReport> GetFullOutcomeReport(string userId, DateTime dateFrom, DateTime dateTo)
        {
            var categories = await _parentTransactionCategoryRepository.GetAllParentCategories(userId);
            var user = await _userRepository.GetConcreteUser(userId);
            var totalSum = await _repCalculatorService.CalculateTotal(userId, Domain.Enums.LedgerType.Credit, dateFrom, dateTo);

            List<OutcomeCategoryReport> outcomeCategoryReports = await _repItemsService.GetCategoryReportListByCategory(categories, dateFrom, dateTo);

            OutcomeReport outcomeReport = new OutcomeReport
            {
                CategoriesReport = outcomeCategoryReports,
                Currency = user.MainCurrencyId,
                TotalSum = Math.Round(totalSum, 2),
            };

            return outcomeReport;
        }
    }
}
