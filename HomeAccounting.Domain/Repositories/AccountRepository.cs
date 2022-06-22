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
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext _databaseContext;
        private const string ALL_TYPE = "All";

        public AccountRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateAccount(Account account)
        {
            await _databaseContext.Accounts.AddAsync(account);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAccount(int accountToDeleteId)
        {
            var accountToDelete = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountToDeleteId);

            if (accountToDelete != null)
            {
                _databaseContext.Remove(accountToDelete);
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task EditAccount(Account newAccount, int accountToEditId)
        {
            var accountToEdit = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountToEditId); 

            if (accountToEdit != null)
            {
                if (newAccount.Name != null)
                {
                    accountToEdit.Name = newAccount.Name;
                }

                if (newAccount.CurrencyId != null)
                {
                    accountToEdit.CurrencyId = newAccount.CurrencyId;
                }

                if (newAccount.TransactionCategoryId != null)
                {
                    accountToEdit.TransactionCategoryId = newAccount.TransactionCategoryId;
                }
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByCategory(int categoryId)
        {
            return await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.TransactionCategoryId == categoryId);
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);
        }

        public async Task<IEnumerable<Account>> GetAllAccountByUser(string userId)
        {
            return await _databaseContext.Accounts.Where(x => x.AppUserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAllAcountsByType(string userId, string type)
        {
            if (type == ALL_TYPE)
            {
                return await _databaseContext.Accounts.Where(x => x.AppUserId == userId).ToListAsync();
            }

            return await _databaseContext.Accounts.Where(x => x.AppUserId == userId && x.Type == type).ToListAsync();
        }
    }
}
