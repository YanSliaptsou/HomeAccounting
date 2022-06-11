using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/categories")]
    public class CategoriesController : BaseApiController
    {
        private readonly ITransactionCategoryRepository _transactionCategoryRepository;
        private readonly ICategoryService _categoryService;
        public CategoriesController(ITransactionCategoryRepository transactionCategoryRepository, ICategoryService categoryService)
        {
            _transactionCategoryRepository = transactionCategoryRepository;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionCategory>>> GetTransactionCategories()
        {
            return Ok(await _transactionCategoryRepository.GetAllCategories());
        }

        [Route("list-except-repeated")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionCategory>>> ExceptTransactionCategoriesLocatedInAccounts()
        {
            var categories = await _categoryService.ExceptTransactionCategoriesLocatedInAccounts(User.GetUserId());
            return Ok(categories);
        }

        [Route("list-by-parent-category/{parentCategoryId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionCategory>>> GetTransactionCategoriesByParentCategory(int parentCategoryId)
        {
            return Ok(await _transactionCategoryRepository.GetAllCategoiesByParentCategory(parentCategoryId));
        }

        [Route("list-by-user")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionCategory>>> GetTransactionCategoriesByUserId()
        {
            return Ok(await _transactionCategoryRepository.GetAllCategoriesByUser(User.GetUserId()));
        }

        [Route("{transactionCategoryId}")]
        [HttpGet]
        public async Task<ActionResult<TransactionCategory>> GetTransactionCategory(int transactionCategoryId)
        {
            var transactCategory = await _transactionCategoryRepository.GetConcreteTransactionCategory(transactionCategoryId);
            if (transactCategory == null)
            {
                return BadRequest("Such category does not exists");
            }

            return Ok(transactCategory);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransactionCategory([FromBody] TransactionCategory transactionCategory)
        {
            var userId = User.GetUserId();
            if (!(await _categoryService.IsSuchCategoryExists(userId, transactionCategory.Name)))
            {
                return BadRequest("Such category name is already exists");
            }

            transactionCategory.UserId = userId;
            await _transactionCategoryRepository.CreateTransactionCategory(transactionCategory);
            return Ok(transactionCategory);
        }

        [Route("{transactionCategoryToEdit}")]
        [HttpPut]
        public async Task<ActionResult> EditParentTransactionCategory([FromBody] TransactionCategory transactionCategory, int transactionCategoryToEdit)
        {
            await _transactionCategoryRepository.EditTransactionCategory(transactionCategory, transactionCategoryToEdit);

            return Ok();
        }

        [Route("{transactionCategoryToDeleteId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteParentCategoryRepository(int transactionCategoryToDeleteId)
        {
            await _transactionCategoryRepository.DeleteTransactionCategory(transactionCategoryToDeleteId);

            return Ok();
        }

    }
}
