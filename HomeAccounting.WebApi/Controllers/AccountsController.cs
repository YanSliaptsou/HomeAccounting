using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountService accountService, IAccountRepository accountRepository)
        {
            _accountService = accountService;
            _accountRepository = accountRepository;
        }
        
        [Route("list-by-user")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountByUser()
        {
            return Ok(await _accountRepository.GetAllAccountByUser(User.GetUserId()));
        }

        [Route("list-by-type/{type}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAcountsByType(string type)
        {
            return Ok(await _accountRepository.GetAllAcountsByType(User.GetUserId(), type));
        }

        [Route("account-by-id/{accountId}")]
        [HttpGet]
        public async Task<ActionResult<Account>> GetAccountById(int accountId)
        {
            return Ok(await _accountRepository.GetAccountById(accountId));
        }

        [Route("account-by-category/{categoryId}")]
        [HttpGet]
        public async Task<ActionResult<Account>> GetAccountByCategory(int categoryId)
        {
            return Ok(await _accountRepository.GetAccountByCategory(categoryId));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount([FromBody] Account account)
        {
            var userId = User.GetUserId();
            if (!(await _accountService.IsSuchAccountNameExists(userId, account.Name)))
            {
                return BadRequest("Such account name is already exists");
            }

            account.AppUserId = userId;

            if (await _accountService.CreateNameForOutcome(account, (int)account.TransactionCategoryId) != string.Empty)
            {
                account.Name = await _accountService.CreateNameForOutcome(account, (int)account.TransactionCategoryId);
            }

            await _accountRepository.CreateAccount(account);
            return Ok(account);
        }

        [Route("{accountToEditId}")]
        [HttpPut]
        public async Task<ActionResult> EditAccount([FromBody] Account newAccount, int accountToEditId)
        {
            await _accountRepository.EditAccount(newAccount, accountToEditId);

            return Ok();
        }

        [Route("{accountToDeleteId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAccount(int accountToDeleteId)
        {
            await _accountRepository.DeleteAccount(accountToDeleteId);

            return Ok();
        }

    }
}
