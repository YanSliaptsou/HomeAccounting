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

        public async Task CreateTransactionCategory(TransactionCategory transactionCategory)
        {
            await _context.TransactionCategories.AddAsync(transactionCategory);
        }

        public async Task DeleteTransactionCategory(int transactionCategoryid)
        {
            var transactionCategory = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryid);
            if (transactionCategory != null)
            {
                _context.TransactionCategories.Remove(transactionCategory);
            }
        }

        public async Task EditTransactionCategory(TransactionCategory transactionCategoryEditable, int transactionCategoryId)
        {
            var transactionCategory = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryId);
            if (transactionCategory != null)
            {
                if (transactionCategory.Name != null)
                {
                    transactionCategory.Name = transactionCategoryEditable.Name;
                }

                if (transactionCategory.ParentTransactionCategoryId != null)
                {
                    transactionCategory.ParentTransactionCategoryId = transactionCategoryEditable.ParentTransactionCategoryId;
                }
            }
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllCategories()
        {
            return await _context.TransactionCategories.ToListAsync();
        }

        public async Task<TransactionCategory> GetConcreteTransactionCategory(int transactionCategoryId)
        {
            return await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryId);
        }
    }
}
