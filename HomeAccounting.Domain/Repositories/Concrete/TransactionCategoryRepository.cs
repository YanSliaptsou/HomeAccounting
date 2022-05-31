using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.DTOs.CreateDTOs;
using HomeAccounting.Domain.DTOs.ViewDTOs;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Abstarct;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Concrete
{
    public class TransactionCategoryRepository : ITransactionCategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public TransactionCategoryRepository(IMapper mapper, DatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateTransactionCategory(TransactionCategoryCreateDto createDto)
        {
            var transactionCategory = _mapper.Map<TransactionCategory>(createDto);
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

        public async Task EditTransactionCategory(TransactionCategoryCreateDto createDto, int transactionCategoryId)
        {
            var transactionCategory = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryId);
            if (transactionCategory != null)
            {
                if (createDto.Name != null)
                {
                    transactionCategory.Name = createDto.Name;
                }

                if (createDto.Constraint != null)
                {
                    transactionCategory.Constraint = createDto.Constraint;
                }

                if (createDto.ParentTransactionCategoryId != null)
                {
                    transactionCategory.ParentTransactionCategory = await _context.ParentTransactionCategories
                    .FirstOrDefaultAsync(x => x.Id == createDto.ParentTransactionCategoryId);
                }
            }
        }

        public async Task<IEnumerable<TransactionCategoryViewDTO>> GetAllCategories()
        {
            return await _context.TransactionCategories.ProjectTo<TransactionCategoryViewDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<TransactionCategoryViewDTO> GetConcreteTransactionCategory(int transactionCategoryId)
        {
            var transactionCategory = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == transactionCategoryId);

            return _mapper.Map<TransactionCategoryViewDTO>(transactionCategory);
        }
    }
}
