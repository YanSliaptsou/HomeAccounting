using HomeAccounting.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountByUser(string userId);
        Task<IEnumerable<Account>> GetAllAcountsByType(string userId, string type);
        Task<Account> GetAccountById(int accountId);
        Task<Account> GetAccountByCategory(int categoryId);
        Task CreateAccount(Account account);
        Task EditAccount(Account newAccount, int accountToEditId);
        Task DeleteAccount(int accountToDeleteId);
    }
}
