using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface ILegderService
    {
        Task<IEnumerable<Ledger>> GetLedgers(string userId, LedgerType? type = null, int? accountFromId = null, int? accountToId = null);
        Task<Ledger> GetConcreteLedger(int ledgerId);
        Task CreateLegder(Ledger ledger);
        Task EditLegder(int legderId, Ledger ledgerToEdit);
        Task DeleteLegder(int legderToDeleteId);
    }
}
