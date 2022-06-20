using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.WebApi.DTOs;
using HomeAccounting.WebApi.DTOs.RegistrationDTOs;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Extensions;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.WebApi.Controllers.BaseController;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models.Entities.Reports;
using HomeAccounting.WebApi.DTOs.ReportDto;
using HomeAccounting.WebApi.DTOs.ReportDto.Income;
using HomeAccounting.WebApi.DTOs.ReportDto.Outcome;
using HomeAccounting.WebApi.DTOs.UserDto;
using HomeAccounting.WebApi.DTOs.ParentCategoriesDTO;
using HomeAccounting.WebApi.DTOs.CategoriesDto;
using HomeAccounting.WebApi.DTOs.AccountsDTO;
using HomeAccounting.Domain.Models.Entities;

namespace HomeAccounting.WebApi.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserRegistrationRequestDTO, AppUser>();
            CreateMap<Ledger, LedgerResponseDto>()
                .ForMember(x => x.AccountNameFrom, x => x.MapFrom(x => x.AccountFrom.Name))
                .ForMember(x => x.AccountNameTo, x => x.MapFrom(x => x.AccountTo.Name))
                .ForMember(x => x.CurrencyFrom, x => x.MapFrom(x => x.AccountFrom.CurrencyId))
                .ForMember(x => x.CurrencyTo, x => x.MapFrom(x => x.AccountTo.CurrencyId));
            CreateMap<LegderSendDto, Ledger>()
                .ForMember(x => x.Type, x => x.MapFrom(src => src.Type == 0 ? LedgerType.Debet : LedgerType.Credit))
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.AccountFrom, x => x.Ignore())
                .ForMember(x => x.AccountTo, x => x.Ignore())
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.UserId, x => x.Ignore());
            CreateMap<AccountReport, AccountReportDto>();
            CreateMap<IncomeReport, IncomeReportDto>();
            CreateMap<OutcomeReport, OutcomeReportDto>();
            CreateMap<OutcomeCategoryReport, OutcomeCategoryReportDto>();
            CreateMap<AppUser, UserResponseDto>();
            CreateMap<UserRequestDto, AppUser>()
                .ForMember(x => x.AccessFailedCount, x => x.Ignore())
                .ForMember(x => x.ConcurrencyStamp, x => x.Ignore())
                .ForMember(x => x.Email, x => x.Ignore())
                .ForMember(x => x.EmailConfirmed, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.LockoutEnabled, x => x.Ignore())
                .ForMember(x => x.LockoutEnd, x => x.Ignore())
                .ForMember(x => x.MainCurrency, x => x.Ignore())
                .ForMember(x => x.NormalizedEmail, x => x.Ignore())
                .ForMember(x => x.NormalizedUserName, x => x.Ignore())
                .ForMember(x => x.PasswordHash, x => x.Ignore())
                .ForMember(x => x.PhoneNumber, x => x.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, x => x.Ignore())
                .ForMember(x => x.SecurityStamp, x => x.Ignore())
                .ForMember(x => x.TwoFactorEnabled, x => x.Ignore());
            CreateMap<ParentTransactionCategory, ParentCategoryResponseDto>();
            CreateMap<ParentCategoryRequestDto, ParentTransactionCategory>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.UserId, x => x.Ignore());
            CreateMap<TransactionCategory, CategoryResponseDto>();
            CreateMap<CategoryRequestDto, TransactionCategory>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.ParentTransactionCategory, x => x.Ignore())
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.UserId, x => x.Ignore());
            CreateMap<Account, AccountResponseDto>();
            CreateMap<AccountRequestDto, Account>()
                .ForMember(x => x.AppUser, x => x.Ignore())
                .ForMember(x => x.AppUserId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.TransactionCategory, x => x.Ignore())
                .ForMember(x => x.Сurrency, x => x.Ignore());
        }
    }
}
