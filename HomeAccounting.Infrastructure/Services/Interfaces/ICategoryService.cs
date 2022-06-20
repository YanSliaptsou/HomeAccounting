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
        Task<bool> IsSuchCategoryExists(string userId, int id);
        Task<IEnumerable<TransactionCategory>> ExceptTransactionCategoriesLocatedInAccounts(string userId);
        Task<TransactionCategory> GetConcreteTransactionCategory(int transactionCategoryId);
        Task<IEnumerable<TransactionCategory>> GetAllCategoriesByUser(string userId);
        Task<IEnumerable<TransactionCategory>> GetAllCategoiesByParentCategory(int parentCategoryId);
        Task CreateTransactionCategory(TransactionCategory transactionCategory);
        Task DeleteTransactionCategory(int transactionCategoryToDeleteId);
        Task EditTransactionCategory(TransactionCategory newTransactionCategory, int transactionCategoryToEditId);
    }
}
