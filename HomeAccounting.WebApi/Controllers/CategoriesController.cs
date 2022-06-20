using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using HomeAccounting.WebApi.DTOs.CategoriesDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/categories")]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private const string CATEGORY_NOT_EXISTS_MESSAGE = "Such category does not exists";
        private const string CATEGORY_NAME_EXISTS_MESSAGE = "Such category name is already exists";
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [Route("list-except-repeated")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> ExceptTransactionCategoriesLocatedInAccounts()
        {
            var categories = await _categoryService.ExceptTransactionCategoriesLocatedInAccounts(User.GetUserId());
            var categoriesResponse = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(new Response<IEnumerable<CategoryResponseDto>>
            {
                Data = categoriesResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            }) ;
        }

        [Route("list-by-parent-category/{parentCategoryId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetTransactionCategoriesByParentCategory(int parentCategoryId)
        {
            var categories = await _categoryService.GetAllCategoiesByParentCategory(parentCategoryId);
            var categoriesResponse = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(new Response<IEnumerable<CategoryResponseDto>>
            {
                Data = categoriesResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("list-by-user")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetTransactionCategoriesByUserId()
        {
            var categories = await _categoryService.GetAllCategoriesByUser(User.GetUserId());
            var categoriesResponse = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(new Response<IEnumerable<CategoryResponseDto>>
            {
                Data = categoriesResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("{transactionCategoryId}")]
        [HttpGet]
        public async Task<ActionResult<CategoryResponseDto>> GetTransactionCategory(int transactionCategoryId)
        {
            var transactCategory = await _categoryService.GetConcreteTransactionCategory(transactionCategoryId);
            if (await _categoryService.IsSuchCategoryExists(User.GetUserId(), transactionCategoryId))
            {
                return BadRequest(new Response<CategoryResponseDto> 
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = CATEGORY_NOT_EXISTS_MESSAGE,
                    IsSuccessful = false
                });
            }

            var transactCatResponse = _mapper.Map<CategoryResponseDto>(transactCategory);

            return Ok(new Response<CategoryResponseDto> 
            {
                IsSuccessful = true,
                Data = transactCatResponse,
                ErrorCode = null,
                ErrorMessage = null
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransactionCategory([FromBody] CategoryRequestDto categoryRequestDto)
        {
            if (!(await _categoryService.IsSuchCategoryExists(User.GetUserId(), categoryRequestDto.Name)))
            {
                return BadRequest(new Response<CategoryRequestDto> 
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = CATEGORY_NAME_EXISTS_MESSAGE,
                    IsSuccessful = false
                });
            }

            var category = _mapper.Map<TransactionCategory>(categoryRequestDto);
            category.UserId = User.GetUserId();
            await _categoryService.CreateTransactionCategory(category);
            return Ok();
        }

        [Route("{transactionCategoryToEdit}")]
        [HttpPut]
        public async Task<ActionResult> EditParentTransactionCategory([FromBody] CategoryRequestDto categoryRequestDto, int transactionCategoryToEdit)
        {
            var transactionCategory = _mapper.Map<TransactionCategory>(categoryRequestDto);
            await _categoryService.EditTransactionCategory(transactionCategory, transactionCategoryToEdit);

            return Ok();
        }

        [Route("{transactionCategoryToDeleteId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteParentCategoryRepository(int transactionCategoryToDeleteId)
        {
            await _categoryService.DeleteTransactionCategory(transactionCategoryToDeleteId);

            return Ok();
        }

    }
}
