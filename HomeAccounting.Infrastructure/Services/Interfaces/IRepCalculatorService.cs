using HomeAccounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface IRepCalculatorService
    {
        Task<decimal> CalculateByCategory(int categoryId, DateTime dateFrom, DateTime dateTo);
        Task<decimal> CalculateByAccount(int accountId, DateTime dateFrom, DateTime dateTo);
        Task<decimal> CalculateTotal(string userId, LedgerType type, DateTime dateFrom, DateTime dateTo);
        Task<decimal> CalculatePercentageByCategory(int categoryId, DateTime dateFrom, DateTime dateTo);
        Task<decimal> CalculatePercentageByAccount(int accountId, DateTime dateFrom, DateTime dateTo);
    }
}
