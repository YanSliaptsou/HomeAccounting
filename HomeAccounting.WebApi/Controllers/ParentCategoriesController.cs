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
    [Route("api/parent-categories")]
    public class ParentCategoriesController : BaseApiController
    {
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;
        private readonly IParentCategoryService _parentCategoryService;

        public ParentCategoriesController(IParentTransactionCategoryRepository parentTransactionCategoryRepository,
                                          IParentCategoryService parentCategoryService)
        {
            _parentTransactionCategoryRepository = parentTransactionCategoryRepository;
            _parentCategoryService = parentCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentTransactionCategory>>> GetParentTransactionCategories()
        {
            return Ok(await _parentTransactionCategoryRepository.GetAllParentCategories(User.GetUserId()));
        }

        [Route("{parentCategoryId}")]
        [HttpGet]
        public async Task<ActionResult<ParentTransactionCategory>> GetParentCategory(int parentCategoryId)
        {
            var parentCategory = await _parentTransactionCategoryRepository.GetParentCategory(parentCategoryId);
            if (parentCategory == null)
            {
                return BadRequest("Such parent category does not exists");
            }
            else
            {
                return Ok(parentCategory);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateParentTransactionCategory([FromBody] ParentTransactionCategory parentTransactionCategory)
        {
            var userId = User.GetUserId();
            if (!(await _parentCategoryService.IsSuchParentCategoryExists(userId, parentTransactionCategory.Name)))
            {
                return BadRequest("Such parent category name is already exists");
            }

            parentTransactionCategory.UserId = userId;
            await _parentTransactionCategoryRepository.CreateParentTransactionCategory(parentTransactionCategory);

            return Ok(parentTransactionCategory);
        }

        [Route("{parentTransactionCategoryToEdit}")]
        [HttpPut]
        public async Task<ActionResult> EditParentTransactionCategory ([FromBody] ParentTransactionCategory parentTransactionCategory, int parentTransactionCategoryToEdit)
        {
            await _parentTransactionCategoryRepository.EditParentTransactionCategory(parentTransactionCategory, parentTransactionCategoryToEdit);

            return Ok();
        }

        [Route("{parentTransactionCategoryToDeleteId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteParentCategoryRepository (int parentTransactionCategoryToDeleteId)
        {
            await _parentTransactionCategoryRepository.DeleteParentTransactionCategory(parentTransactionCategoryToDeleteId);

            return Ok();
        }
    }
}

