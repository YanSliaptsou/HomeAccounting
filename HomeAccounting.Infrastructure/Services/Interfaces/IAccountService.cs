using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface IAccountService
    {
        Task<bool> IsSuchAccountNameExists(string userId, string name);
        Task<LedgerType> DefineAccountType(int accountId);
        Task<List<Account>> GetAccountsListByCategory(int categoryId);
    }
}
