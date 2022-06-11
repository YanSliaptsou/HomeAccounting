using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Abstract;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;
        private readonly ITransactionCategoryRepository _transactionCategoryRepository;

        public AccountService(IAccountRepository accountRepository, IParentTransactionCategoryRepository parentTransactionCategoryRepository,
            ITransactionCategoryRepository transactionCategoryRepository)
        {
            _accountRepository = accountRepository;
            _parentTransactionCategoryRepository = parentTransactionCategoryRepository;
            _transactionCategoryRepository = transactionCategoryRepository;
        }

        public async Task<string> CreateNameForOutcome(Account account, int categoryId)
        {
            if (account.Type == "Outcome")
            {
                var subcategory = _transactionCategoryRepository.GetConcreteTransactionCategory(categoryId).Result;
                string subcategoryName = subcategory.Name;

                var parentCategory = _parentTransactionCategoryRepository.GetParentCategory((int)subcategory.ParentTransactionCategoryId);
                string parentCategoryName = parentCategory.Result.Name;

                return parentCategoryName + "/" + subcategoryName;
            }

            return string.Empty;
        }

        public async Task<bool> IsSuchAccountNameExists(string userId, string name)
        {
            var account = _accountRepository
                .GetAllAccountByUser(userId)
                .Result
                .FirstOrDefault(x => x.Name == name);

            return account == null;
        }
    }
}
