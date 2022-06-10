using HomeAccounting.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface ILimitsRepository
    {
        Task<IEnumerable<OutcomeLimit>> GetAllLimits();
        Task<IEnumerable<OutcomeLimit>> GetLimitsByAccount(int accountId);
        Task<OutcomeLimit> GetConcreteLimit(int limitId);
        Task CreateLimit(OutcomeLimit outcomeLimit);
        Task EditLimit(OutcomeLimit newLimit, int limitToEditId);
        Task DeleteLimit(int limitId);
    }
}
