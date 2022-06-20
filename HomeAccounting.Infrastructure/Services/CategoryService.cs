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
        private readonly IAccountService _accountService;
        public CategoryService(ITransactionCategoryRepository transactionCategoryRepository, IAccountRepository accountRepository, IAccountService accountService)
        {
            _transactionCategoryRepository = transactionCategoryRepository;
            _accountRepository = accountRepository;
            _accountService = accountService;
        }

        public async Task CreateTransactionCategory(TransactionCategory transactionCategory)
        {
           await _transactionCategoryRepository.CreateTransactionCategory(transactionCategory);
        }

        public async Task DeleteTransactionCategory(int transactionCategoryToDeleteId)
        {
            var account = await _accountRepository.GetAccountByCategory(transactionCategoryToDeleteId);
            if (account != null)
            {
                await _accountService.DeleteAccount(account.Id);
            }
            await _transactionCategoryRepository.DeleteTransactionCategory(transactionCategoryToDeleteId);
        }

        public async Task EditTransactionCategory(TransactionCategory newTransactionCategory, int transactionCategoryToEditId)
        {
            await _transactionCategoryRepository.EditTransactionCategory(newTransactionCategory, transactionCategoryToEditId);
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

        public async Task<IEnumerable<TransactionCategory>> GetAllCategoiesByParentCategory(int parentCategoryId)
        {
            return await _transactionCategoryRepository.GetAllCategoiesByParentCategory(parentCategoryId);
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllCategoriesByUser(string userId)
        {
            return await _transactionCategoryRepository.GetAllCategoriesByUser(userId);
        }

        public async Task<TransactionCategory> GetConcreteTransactionCategory(int transactionCategoryId)
        {
            return await _transactionCategoryRepository.GetConcreteTransactionCategory(transactionCategoryId); 
        }

        public async Task<bool> IsSuchCategoryExists(string userId, string categoryName)
        {
            var category = _transactionCategoryRepository
                .GetAllCategoriesByUser(userId)
                .Result
                .FirstOrDefault(x => x.Name == categoryName);

            return category == null;
        }

        public async Task<bool> IsSuchCategoryExists(string userId, int id)
        {
            var category = _transactionCategoryRepository
                .GetAllCategoriesByUser(userId)
                .Result
                .FirstOrDefault(x => x.Id == id);

            return category == null;
        }
    }
}
