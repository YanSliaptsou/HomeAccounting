﻿using AutoMapper;
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
        }
    }
}
