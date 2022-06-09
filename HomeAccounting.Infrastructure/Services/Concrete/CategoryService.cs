using HomeAccounting.Domain.Models;
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
        private readonly IAccountRepository _accountRepository;
        public CategoryService(ITransactionCategoryRepository transactionCategoryRepository, IAccountRepository accountRepository)
        {
            _transactionCategoryRepository = transactionCategoryRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<TransactionCategory>> ExceptTransactionCategoriesLocatedInAccounts(string userId)
        {
            var transactionCategories = _transactionCategoryRepository.GetAllCategoriesByUser(userId).Result;
            var categoriesList = transactionCategories.ToList();

            var newCategoriesList = new List<TransactionCategory>();

            foreach (var category in categoriesList)
            {
                var account = _accountRepository.GetAllAccountByUser(userId).Result.FirstOrDefault(x => x.TransactionCategoryId == category.Id);
                if (account == null)
                {
                    newCategoriesList.Add(category);
                }
            }

            return newCategoriesList;
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
