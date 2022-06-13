using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class LimitsService : ILimitsService
    {
        private readonly ILimitsRepository _limitsRepository;
        private readonly ILegderRepository _legderRepository;

        public LimitsService(ILimitsRepository limitsRepository, ILegderRepository legderRepository)
        {
            _limitsRepository = limitsRepository;
            _legderRepository = legderRepository;
        }

        public async Task<decimal> CalculatePercentage(int limitId)
        {
            var limit = await _limitsRepository.GetConcreteLimit(limitId);
            var totalSpend = await CalculateTotalSpend(limit.AccountId, (DateTime)limit.LimitFrom, (DateTime)limit.LimitTo);

            return Math.Round((totalSpend / limit.Limit) * 100,2);
        }

        public async Task<decimal> CalculateTotalSpend(int accountToId, DateTime dateFrom, DateTime dateTo)
        {
            var ledgersSum = _legderRepository.GetAllLegdersByAccountTo(accountToId).Result
                .Where(x => x.DateTime >= dateFrom && x.DateTime <= dateTo).Sum(x => x.AmmountTo);

            return ledgersSum;
        }
    }
}
