using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using HomeAccounting.WebApi.DTOs;
using HomeAccounting.WebApi.DTOs.ParentCategoriesDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/parent-categories")]
    public class ParentCategoriesController : BaseApiController
    {
        private readonly IParentCategoryService _parentCategoryService;
        private readonly IMapper _mapper;
        private const string CATEGORY_NOT_EXISTS_MESSAGE = "Such parent category does not exists";
        private const string CATEGORY_NAME_EXISTS_MESSAGE = "Such parent category name is already exists";
        public ParentCategoriesController(IParentCategoryService parentCategoryService, IMapper mapper)
        {
            _parentCategoryService = parentCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentCategoryResponseDto>>> GetParentTransactionCategories()
        {
            var parentCategories = await _parentCategoryService.GetAllParentCategories(User.GetUserId());
            var parentCategoriesResponse = _mapper.Map<IEnumerable<ParentCategoryResponseDto>>(parentCategories);
            return Ok(new Response<IEnumerable<ParentCategoryResponseDto>>
            {
                Data = parentCategoriesResponse,
                ErrorCode = null,
                ErrorMessage = null,
                IsSuccessful = true
            });
        }

        [Route("{parentCategoryId}")]
        [HttpGet]
        public async Task<ActionResult<ParentCategoryResponseDto>> GetParentCategory(int parentCategoryId)
        {
            var parentCategory = await _parentCategoryService.GetParentCategory(parentCategoryId);
            if (await _parentCategoryService.IsSuchParentCategoryExists(User.GetUserId(), parentCategoryId))
            {
                return BadRequest(new Response<ParentCategoryResponseDto> 
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    ErrorMessage = CATEGORY_NOT_EXISTS_MESSAGE,
                    IsSuccessful = false
                });
            }
            else
            {
                var parentCategoryRespnse = _mapper.Map<ParentCategoryResponseDto>(parentCategory);
                return Ok(new Response<ParentCategoryResponseDto> 
                {
                    Data = parentCategoryRespnse,
                    ErrorCode = null,
                    ErrorMessage = null,
                    IsSuccessful = true
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateParentTransactionCategory([FromBody] ParentCategoryRequestDto parentCategoryRequestDto)
        {
            var parentTransactionCategory = _mapper.Map<ParentTransactionCategory>(parentCategoryRequestDto);
            if (!(await _parentCategoryService.IsSuchParentCategoryExists(User.GetUserId(), parentTransactionCategory.Name)))
            {
                return BadRequest(new Response<ParentTransactionCategory>
                {
                    Data = null,
                    ErrorCode = HttpStatusCode.BadRequest.ToString(),
                    IsSuccessful = false,
                    ErrorMessage = CATEGORY_NAME_EXISTS_MESSAGE
                });
            }

            parentTransactionCategory.UserId = User.GetUserId();
            await _parentCategoryService.CreateParentTransactionCategory(parentTransactionCategory);

            return Ok();
        }

        [Route("{parentTransactionCategoryToEdit}")]
        [HttpPut]
        public async Task<ActionResult> EditParentTransactionCategory ([FromBody] ParentCategoryRequestDto parentCategoryRequestDto, int parentTransactionCategoryToEdit)
        {
            var parentTransactionCategory = _mapper.Map<ParentTransactionCategory>(parentCategoryRequestDto);
            await _parentCategoryService.EditParentTransactionCategory(parentTransactionCategory, parentTransactionCategoryToEdit);

            return Ok();
        }

        [Route("{parentTransactionCategoryToDeleteId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteParentCategoryRepository(int parentTransactionCategoryToDeleteId)
        {
            await _parentCategoryService.DeleteParentTransactionCategory(parentTransactionCategoryToDeleteId);

            return Ok();
        }
    }
}

