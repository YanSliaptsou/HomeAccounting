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
        /*Task<IncomeAccountReport> GetIncomeAccountReport(int accountId, DateTime dateFrom, DateTime dateTo);
        Task<OutcomeAccountReport> GetOutcomeAccountReport(int accountId, DateTime dateFrom, DateTime dateTo);*/
        Task<OutcomeCategoryReport> GetOutcomeCategoryReport(int categoryId, DateTime dateFrom, DateTime dateTo);
        Task<AccountReport> GetAccountReport(int accountId, DateTime dateFrom, DateTime dateTo);
    }
}
