using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories
{
    public class LimitsRepository : ILimitsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public LimitsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateLimit(OutcomeLimit outcomeLimit)
        {
            await _databaseContext.OutcomeLimits.AddAsync(outcomeLimit);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteLimit(int limitId)
        {
            var limit = await _databaseContext.OutcomeLimits.FirstOrDefaultAsync(x => x.Id == limitId);
            _databaseContext.OutcomeLimits.Remove(limit);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task EditLimit(OutcomeLimit newLimit, int limitToEditId)
        {
            var limit = await _databaseContext.OutcomeLimits.FirstOrDefaultAsync(x => x.Id == limitToEditId);
            if (newLimit != null)
            {
                if(newLimit.LimitFrom != null)
                {
                    limit.LimitFrom = newLimit.LimitFrom;
                }

                if(newLimit.LimitTo != null)
                {
                    limit.LimitTo = newLimit.LimitTo;
                }

                if(newLimit.Limit != null)
                {
                    limit.Limit = newLimit.Limit;
                }
            }

            await _databaseContext.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<OutcomeLimit>> GetAllLimits()
        {
            return await _databaseContext.OutcomeLimits.ToListAsync();
        }

        public async Task<OutcomeLimit> GetConcreteLimit(int limitId)
        {
            return await _databaseContext.OutcomeLimits.FirstOrDefaultAsync(x => x.Id == limitId);
        }

        public async Task<IEnumerable<OutcomeLimit>> GetLimitsByAccount(int accountId)
        {
            return await _databaseContext.OutcomeLimits.Where(x => x.AccountId == accountId).ToListAsync();
        }
    }
}
