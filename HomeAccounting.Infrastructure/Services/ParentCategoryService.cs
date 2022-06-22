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
    public class ParentCategoryService : IParentCategoryService
    {
        private readonly IParentTransactionCategoryRepository _parentTransactionCategoryRepository;
        private readonly ICategoryService _categoryService;

        public ParentCategoryService(IParentTransactionCategoryRepository parentTransactionCategoryRepository,
            ICategoryService categoryService)
        {
            _parentTransactionCategoryRepository = parentTransactionCategoryRepository;
            _categoryService = categoryService;
        }

        public async Task<bool> IsSuchParentCategoryExists(string userId, string parentCategoryName)
        {
            var category = _parentTransactionCategoryRepository
                .GetAllParentCategories(userId)
                .Result
                .FirstOrDefault(x => x.Name == parentCategoryName);

            return category == null;
        }

        public async Task<bool> IsSuchParentCategoryExists(string userId, int id)
        {
            var category = _parentTransactionCategoryRepository
                .GetAllParentCategories(userId)
                .Result
                .FirstOrDefault(x => x.Id == id);
            return category == null;
        }

        public async Task CreateParentTransactionCategory(ParentTransactionCategory parentTransactionCategory)
        {
            await _parentTransactionCategoryRepository.CreateParentTransactionCategory(parentTransactionCategory);
        }

        public async Task DeleteParentTransactionCategory(int parentTransactionCategoryToDeleteId)
        {
            var categories = await _categoryService.GetAllCategoiesByParentCategory(parentTransactionCategoryToDeleteId);
            var catList = categories.ToList();

            for(int i = 0; i < catList.Count(); i++)
            {
                if (catList[i] != null)
                {
                   await _categoryService.DeleteTransactionCategory(catList[i].Id);
                }
            }

            await _parentTransactionCategoryRepository.DeleteParentTransactionCategory(parentTransactionCategoryToDeleteId);
        }

        public async Task EditParentTransactionCategory(ParentTransactionCategory parentTransactionCategory, int parentTransactionCategoryToEdit)
        {
            await _parentTransactionCategoryRepository.EditParentTransactionCategory(parentTransactionCategory, parentTransactionCategoryToEdit);
        }

        public async Task<IEnumerable<ParentTransactionCategory>> GetAllParentCategories(string userId)
        {
            return await _parentTransactionCategoryRepository.GetAllParentCategories(userId);
        }

        public async Task<ParentTransactionCategory> GetParentCategory(int parentCategoryId)
        {
            return await _parentTransactionCategoryRepository.GetParentCategory(parentCategoryId);
        }
    }
}
