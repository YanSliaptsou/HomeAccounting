using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Models.Entities.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface IRepItemsService
    {
        Task<OutcomeCategoryReport> GetOutcomeCategoryReport(int categoryId, DateTime dateFrom, DateTime dateTo);
        Task<AccountReport> GetAccountReport(int accountId, DateTime dateFrom, DateTime dateTo);
        Task<List<AccountReport>> GetAccountReportListByAccounts(IEnumerable<Account> accounts, DateTime dateFrom, DateTime dateTo);
        Task<List<OutcomeCategoryReport>> GetCategoryReportListByCategory(IEnumerable<ParentTransactionCategory> categories, DateTime dateFrom, DateTime dateTo);
    }
}
