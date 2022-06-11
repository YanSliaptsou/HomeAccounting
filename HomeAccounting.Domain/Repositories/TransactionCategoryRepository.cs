using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class TransactionCategoryRepository : ITransactionCategoryRepository
    {
        private readonly DatabaseContext _context;

        public TransactionCategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllCategoiesByParentCategory(int parentCategoryId)
        {
            var transactCat = await _context.TransactionCategories.Where(x => x.ParentTransactionCategoryId == parentCategoryId).ToListAsync();

            return transactCat;
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllCategories()
        {
            return await _context.TransactionCategories.ToListAsync();
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllCategoriesByUser(string userId)
        {
            return await _context.TransactionCategories.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task CreateTransactionCategory(TransactionCategory transactionCategory)
        {
            await _context.TransactionCategories.AddAsync(transactionCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionCategory(int transactionCategoryid)
        {
            var transactionCategory = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryid);
            if (transactionCategory != null)
            {
                _context.TransactionCategories.Remove(transactionCategory);
            }
            await _context.SaveChangesAsync();
        }

        public async Task EditTransactionCategory(TransactionCategory transactionCategoryEditable, int transactionCategoryId)
        {
            var transactionCategory = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryId);
            if (transactionCategory != null)  
            {
                if (transactionCategoryEditable.Name != null)
                {
                    transactionCategory.Name = transactionCategoryEditable.Name;
                }

                if (transactionCategoryEditable.ParentTransactionCategoryId != null)
                {
                    transactionCategory.ParentTransactionCategoryId = transactionCategoryEditable.ParentTransactionCategoryId;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TransactionCategory> GetConcreteTransactionCategory(int transactionCategoryId)
        {
            return await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryId);
        }
    }
}
