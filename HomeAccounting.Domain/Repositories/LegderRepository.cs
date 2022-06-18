using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories
{
    public class LegderRepository : ILegderRepository
    {
        private readonly DatabaseContext _databaseContext;

        public LegderRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateLegder(Ledger ledger)
        {
            await _databaseContext.Ledgers.AddAsync(ledger);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteLegder(int legderToDeleteId)
        {
            var ledger = await _databaseContext.Ledgers.FirstOrDefaultAsync(x => x.Id == legderToDeleteId);
            if (ledger != null)
            {
                _databaseContext.Ledgers.Remove(ledger);
            }
            await _databaseContext.SaveChangesAsync();
        }

        public async Task EditLegder(int legderId, Ledger ledgerToEdit)
        {
            var ledger = await _databaseContext.Ledgers.FirstOrDefaultAsync(x => x.Id == legderId);
            if (ledger != null)
            {
                if(ledgerToEdit.AccountFromId != null)
                {
                    ledger.AccountFromId = ledgerToEdit.AccountFromId;
                }

                if(ledgerToEdit.AccountToId != null)
                {
                    ledger.AccountToId = ledgerToEdit.AccountToId;
                }

                if(ledgerToEdit.AmmountFrom != null)
                {
                    ledger.AmmountFrom = ledgerToEdit.AmmountFrom;
                }

                if(ledgerToEdit.AmmountTo != null)
                {
                    ledger.AmmountTo = ledgerToEdit.AmmountTo;
                }

                if(ledgerToEdit.DateTime != null)
                {
                    ledger.DateTime = ledgerToEdit.DateTime;
                }
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ledger>> GetAllLegders(string userId)
        {
            var legders = await _databaseContext.Ledgers.Where(x => x.UserId == userId).ToListAsync();
            foreach(var ledger in legders)
            {
                ledger.AccountFrom = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountFromId);
                ledger.AccountTo = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountToId);
            }

            return legders;
        }

        public async Task<IEnumerable<Ledger>> GetAllLegdersByAccountTo(int accountToId)
        {
            var legders = await _databaseContext.Ledgers.Where(x => x.AccountToId == accountToId).ToListAsync();
            foreach (var ledger in legders)
            {
                ledger.AccountFrom = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountFromId);
                ledger.AccountTo = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountToId);
            }

            return legders;
        }

        public async Task<Ledger> GetConcreteLedger(int ledgerId)
        {
            return await _databaseContext.Ledgers.FirstOrDefaultAsync(x => x.Id == ledgerId);
        }

        public async Task<IEnumerable<Ledger>> GetLedgersByAccount(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            var ledgers = await _databaseContext.Ledgers.Where(x => (x.AccountFromId == accountId || x.AccountToId == accountId) 
            && (x.DateTime >= dateFrom && x.DateTime <= dateTo)).OrderBy(x => x.DateTime).ToListAsync();

            foreach(var ledger in ledgers)
            {
                ledger.AccountFrom = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountFromId);
                ledger.AccountTo = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountToId);
            }

            return ledgers;
        }

        public async Task<IEnumerable<Ledger>> GetAllLegdersByType(LedgerType type, string userId)
        {
            var legders = await _databaseContext.Ledgers.Where(x => x.Type == type && x.UserId == userId).OrderBy(x => x.DateTime).ToListAsync();
            foreach (var ledger in legders)
            {
                ledger.AccountFrom = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountFromId);
                ledger.AccountTo = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == ledger.AccountToId);
            }

            return legders;
        }
    }
}
