using HomeAccounting.Domain.Models.Entities.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface IRepConstructorService
    {
        Task<OutcomeReport> GetFullOutcomeReport(string userId, DateTime dateFrom, DateTime dateTo);
        Task<IncomeReport> GetFullIncomeReport(string userId, DateTime dateFrom, DateTime dateTo);
    }
}
