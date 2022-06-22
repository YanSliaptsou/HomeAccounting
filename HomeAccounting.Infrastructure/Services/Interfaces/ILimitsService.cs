using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface ILimitsService
    {
        Task<decimal> CalculateTotalSpend(int accountToId, DateTime dateFrom, DateTime dateTo);
        Task<decimal> CalculatePercentage(int limitId);
    }
}
