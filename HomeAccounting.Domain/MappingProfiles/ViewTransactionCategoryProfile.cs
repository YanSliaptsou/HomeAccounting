using AutoMapper;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.DTOs.CreateDTOs;
using HomeAccounting.Domain.DTOs.ViewDTOs;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.MappingProfiles
{
    public class ViewTransactionCategoryProfile : Profile
    {
        public ViewTransactionCategoryProfile()
        {
            CreateMap<TransactionCategory, TransactionCategoryViewDTO>()
                .ForMember(x => x.ParentCategoryName, opt => opt.MapFrom(src => src.ParentTransactionCategory.Name));
        }
    }
}
