using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface IParentTransactionCategoryRepository
    {
        Task<IEnumerable<ParentTransactionCategory>> GetAllParentCategories(string userId);
        Task<ParentTransactionCategory> GetParentCategory(int parentCategoryId);
        Task CreateParentTransactionCategory(ParentTransactionCategory parentTransactionCategory);
        Task EditParentTransactionCategory(ParentTransactionCategory parentTransactionCategory, int parentTransactionCategoryToEdit);
        Task DeleteParentTransactionCategory(int parentTransactionCategoryToDeleteId);
    }
}
