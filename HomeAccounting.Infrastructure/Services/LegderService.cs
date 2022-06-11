using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class LegderService : ILegderService
    {
        private readonly ILegderRepository _legderRepository;

        public LegderService(ILegderRepository legderRepository)
        {
            _legderRepository = legderRepository;
        }

        public async Task CreateLegder(Ledger ledger)
        {
            await _legderRepository.CreateLegder(ledger);
        }

        public async Task DeleteLegder(int legderToDeleteId)
        {
            await _legderRepository.DeleteLegder(legderToDeleteId);
        }

        public async Task EditLegder(int legderId, Ledger ledgerToEdit)
        {
            await _legderRepository.EditLegder(legderId, ledgerToEdit);
        }

        public async Task<Ledger> GetConcreteLedger(int ledgerId)
        {
            return await _legderRepository.GetConcreteLedger(ledgerId);
        }

        public async Task<IEnumerable<Ledger>> GetLedgers(string userId, LedgerType? type = null, int? accountFromId = null, int? accountToId = null)
        {
            if (type != null)
            {
                return await _legderRepository.GetAllLegdersByType((LedgerType)type, userId);
            }
            else if (accountFromId != null && accountToId != null)
            {
                return await _legderRepository.GetAllLegdersByBothAccounts((int)accountFromId, (int)accountToId);
            }
            else if (accountFromId != null)
            {
                return await _legderRepository.GetAllLegdersByAccountFrom((int)accountFromId);
            }
            else if (accountToId != null)
            {
                return await _legderRepository.GetAllLegdersByAccountTo((int)accountToId);
            }
            else
            {
                return await _legderRepository.GetAllLegders(userId);
            }
        }
    }
}
