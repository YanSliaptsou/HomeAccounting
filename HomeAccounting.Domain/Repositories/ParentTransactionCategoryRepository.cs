using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories
{
    public class ParentTransactionCategoryRepository : IParentTransactionCategoryRepository
    {
        private readonly DatabaseContext _context;

        public ParentTransactionCategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ParentTransactionCategory> GetParentCategory(int parentCategoryId)
        {
            return await _context.ParentTransactionCategories.FirstOrDefaultAsync(x => x.Id == parentCategoryId);
        }

        public async Task CreateParentTransactionCategory(ParentTransactionCategory parentTransactionCategory)
        {
            await _context.ParentTransactionCategories.AddAsync(parentTransactionCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteParentTransactionCategory(int parentTransactionCategoryToDeleteId)
        {
            var parentTransactionCategory = await _context.ParentTransactionCategories.FirstOrDefaultAsync(x => x.Id == parentTransactionCategoryToDeleteId);
            if (parentTransactionCategory != null)
            {
                _context.ParentTransactionCategories.Remove(parentTransactionCategory);
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditParentTransactionCategory(ParentTransactionCategory parentTransactionCategory, int parentTransactionCategoryToEdit)
        {
            var parentCategoryToEdit = await _context.ParentTransactionCategories.FirstOrDefaultAsync(x => x.Id == parentTransactionCategoryToEdit);
            if (parentTransactionCategoryToEdit != null)
            {
                if (parentCategoryToEdit.Name != null)
                {
                    parentCategoryToEdit.Name = parentTransactionCategory.Name;
                }
            }

            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<ParentTransactionCategory>> GetAllParentCategories(string userId)
        {
            return await _context.ParentTransactionCategories.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
