using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Concrete
{
    public class ParentCategoryService : IParentCategoryService
    {
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;

        public ParentCategoryService(IParentTransactionCategoryRepository parentTransactionCategoryRepository)
        {
            this._parentTransactionCategoryRepository = parentTransactionCategoryRepository;
        }

        public async Task<bool> IsSuchParentCategoryExists(string userId, string parentCategoryName)
        {
            var category = _parentTransactionCategoryRepository
                .GetAllParentCategories(userId)
                .Result
                .FirstOrDefault(x => x.Name == parentCategoryName);

            return category == null;
        }
    }
}
