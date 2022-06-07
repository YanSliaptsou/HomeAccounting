using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ITransactionCategoryRepository _transactionCategoryRepository;
        public CategoryService(ITransactionCategoryRepository transactionCategoryRepository)
        {
            _transactionCategoryRepository = transactionCategoryRepository;
        }

        public async Task<bool> IsSuchCategoryExists(string userId, string categoryName)
        {
            var category = _transactionCategoryRepository
                .GetAllCategoriesByUser(userId)
                .Result
                .FirstOrDefault(x => x.Name == categoryName);

            return category == null;
        }
    }
}
