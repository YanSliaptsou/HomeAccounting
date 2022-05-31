using HomeAccounting.Domain.DTOs.ViewDTOs;
using HomeAccounting.Domain.DTOs.CreateDTOs;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Abstarct
{
    public interface ITransactionCategoryRepository
    {
        Task<IEnumerable<TransactionCategoryViewDTO>> GetAllCategories();
        Task<TransactionCategoryViewDTO> GetConcreteTransactionCategory(int transactionCategoryId);
        Task CreateTransactionCategory(TransactionCategoryCreateDto createDto);
        Task DeleteTransactionCategory(int transactionCategoryid);
        Task EditTransactionCategory(TransactionCategoryCreateDto createDto, int transactionCategoryId);
        
    }
}
