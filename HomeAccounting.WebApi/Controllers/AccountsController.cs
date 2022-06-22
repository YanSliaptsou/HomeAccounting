using AutoMapper;
using HomeAccounting.Domain.Models.Entities;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using HomeAccounting.WebApi.DTOs.AccountsDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private const string ACCOUNT_NAME_EXISTS = "Such account name is already exists";

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [Route("{type}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAllAcountsByType(string type)
        {
            var accounts = await _accountService.GetAllAcountsByType(User.GetUserId(), type);
            var accountsResponse = _mapper.Map<IEnumerable<AccountResponseDto>>(accounts);
            return Ok(new Response<IEnumerable<AccountResponseDto>> 
            {
                Data = accountsResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("account-by-id/{accountId}")]
        [HttpGet]
        public async Task<ActionResult<AccountResponseDto>> GetAccountById(int accountId)
        {
            var account = await _accountService.GetAccountById(accountId);
            var accountResponse = _mapper.Map<AccountResponseDto>(account);
            return Ok(new Response<AccountResponseDto> 
            {
                Data = accountResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("account-by-category/{categoryId}")]
        [HttpGet]
        public async Task<ActionResult<AccountResponseDto>> GetAccountByCategory(int categoryId)
        {
            var account = await _accountService.GetAccountByCategory(categoryId);
            var accountResponse = _mapper.Map<AccountResponseDto>(account);
            return Ok(new Response<AccountResponseDto>
            {
                Data = accountResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount([FromBody] AccountRequestDto accountRequest)
        {
            if (!(await _accountService.IsSuchAccountNameExists(User.GetUserId(), accountRequest.Name)))
            {
                return BadRequest(new Response<Account> 
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = ACCOUNT_NAME_EXISTS,
                    IsSuccessful = false
                });
            }

            var account = _mapper.Map<Account>(accountRequest);

            account.AppUserId = User.GetUserId();

            await _accountService.CreateAccount(account);
            return Ok(account);
        }

        [Route("{accountToEditId}")]
        [HttpPut]
        public async Task<ActionResult> EditAccount([FromBody] AccountRequestDto accountRequest, int accountToEditId)
        {
            var account = _mapper.Map<Account>(accountRequest);
            await _accountService.EditAccount(account, accountToEditId);

            return Ok(new Response<AccountRequestDto> 
            {
                Data = accountRequest,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("{accountToDeleteId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAccount(int accountToDeleteId)
        {
            await _accountService.DeleteAccount(accountToDeleteId);

            return Ok();
        }

    }
}
