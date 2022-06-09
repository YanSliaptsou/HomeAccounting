using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface ICategoryService
    {
        Task<bool> IsSuchCategoryExists(string userId, string categoryName);
        Task<IEnumerable<TransactionCategory>> ExceptTransactionCategoriesLocatedInAccounts(string userId);
    }
}
