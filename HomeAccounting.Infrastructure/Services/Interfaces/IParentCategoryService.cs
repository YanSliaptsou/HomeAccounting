using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface IParentCategoryService
    {
        Task<bool> IsSuchParentCategoryExists(string userId, string parentCategoryName);
        Task<bool> IsSuchParentCategoryExists(string userId, int id);
        Task<IEnumerable<ParentTransactionCategory>> GetAllParentCategories(string userId);
        Task<ParentTransactionCategory> GetParentCategory(int parentCategoryId);
        Task CreateParentTransactionCategory(ParentTransactionCategory parentTransactionCategory);
        Task EditParentTransactionCategory(ParentTransactionCategory parentTransactionCategory, int parentTransactionCategoryToEdit);
        Task DeleteParentTransactionCategory(int parentTransactionCategoryToDeleteId);
    }
}
