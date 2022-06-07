using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface ITransactionCategoryRepository
    {
        Task<IEnumerable<TransactionCategory>> GetAllCategories();
        Task<TransactionCategory> GetConcreteTransactionCategory(int transactionCategoryId);
        Task CreateTransactionCategory(TransactionCategory transactionCategory);
        Task DeleteTransactionCategory(int transactionCategoryToDeleteId);
        Task EditTransactionCategory(TransactionCategory newTransactionCategory, int transactionCategoryToEditId);
        
    }
}
