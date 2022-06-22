using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionCategoryRepository _transactionCategoryRepository;
        private readonly ILegderRepository _legderRepository;
        private const string INCOME = "Income";

        public AccountService(IAccountRepository accountRepository, ITransactionCategoryRepository transactionCategoryRepository, ILegderRepository legderRepository)
        {
            _accountRepository = accountRepository;
            _transactionCategoryRepository = transactionCategoryRepository;
            _legderRepository = legderRepository;
        }

        public async Task<LedgerType> DefineAccountType(int accountId)
        {
            return _accountRepository.GetAccountById(accountId).Result.Type == INCOME ? LedgerType.Debet : LedgerType.Credit;
        }

        public async Task<List<Account>> GetAccountsListByCategory(int categoryId)
        {
            List<Account> accountsList = new List<Account>();
            var categories = await _transactionCategoryRepository.GetAllCategoiesByParentCategory(categoryId);

            foreach (var category in categories)
            {
                var account = await _accountRepository.GetAccountByCategory(category.Id);
                accountsList.Add(account);
            }

            return accountsList;
        }

        public async Task<bool> IsSuchAccountNameExists(string userId, string name)
        {
            var account = _accountRepository
                .GetAllAccountByUser(userId)
                .Result
                .FirstOrDefault(x => x.Name == name);

            return account == null;
        }

        public async Task<IEnumerable<Account>> GetAllAccountByUser(string userId)
        {
            return await _accountRepository.GetAllAccountByUser(userId);
        }

        public async Task<IEnumerable<Account>> GetAllAcountsByType(string userId, string type)
        {
            return await _accountRepository.GetAllAcountsByType(userId, type);
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await _accountRepository.GetAccountById(accountId);
        }

        public async Task<Account> GetAccountByCategory(int categoryId)
        {
            return await _accountRepository.GetAccountByCategory(categoryId);
        }

        public async Task CreateAccount(Account account)
        {
            await _accountRepository.CreateAccount(account);
        }

        public async Task EditAccount(Account newAccount, int accountToEditId)
        {
            await _accountRepository.EditAccount(newAccount, accountToEditId);
        }

        public async Task DeleteAccount(int accountToDeleteId)
        {
            var ledgers = await _legderRepository.GetAllLegdersByAccountTo(accountToDeleteId);
            var ledgersList = ledgers.ToList();
            for(int i = 0; i < ledgersList.Count(); i++)
            {
                if (ledgersList[i] != null)
                {
                    await _legderRepository.DeleteLegder(ledgersList[i].Id);
                }
            }

            ledgers = await _legderRepository.GetAllLegdersByAccountFrom(accountToDeleteId);
            ledgersList = ledgers.ToList();
            for (int i = 0; i < ledgersList.Count(); i++)
            {
                if (ledgersList[i] != null)
                {
                    await _legderRepository.DeleteLegder(ledgersList[i].Id);
                }
            }

            await _accountRepository.DeleteAccount(accountToDeleteId);
        }
    }
}
