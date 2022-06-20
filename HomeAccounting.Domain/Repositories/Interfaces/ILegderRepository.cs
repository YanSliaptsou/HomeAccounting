using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface ILegderRepository
    {
        Task<IEnumerable<Ledger>> GetAllLegders(string userId);
        Task<IEnumerable<Ledger>> GetAllLegdersByAccountTo(int accountToId);
        Task<IEnumerable<Ledger>> GetAllLegdersByAccountFrom(int accountFromId);
        Task<IEnumerable<Ledger>> GetLedgersByAccount(int accountId, DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<Ledger>> GetAllLegdersByType(LedgerType type, string userId);
        Task<Ledger> GetConcreteLedger(int ledgerId);
        Task CreateLegder(Ledger ledger);
        Task EditLegder(int legderId, Ledger ledgerToEdit);
        Task DeleteLegder(int legderToDeleteId);
    }
}
