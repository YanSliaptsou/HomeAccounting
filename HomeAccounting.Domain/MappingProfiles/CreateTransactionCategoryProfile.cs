using AutoMapper;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.DTOs.CreateDTOs;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.MappingProfiles
{
    public class CreateTransactionCategoryProfile : Profile
    {
        public CreateTransactionCategoryProfile()
        {

            CreateMap<TransactionCategoryCreateDto, TransactionCategory>();
        }
    }
}
