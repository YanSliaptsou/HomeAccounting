using AutoMapper;
using HomeAccounting.Domain.DTOs.ViewDTOs;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.MappingProfiles
{
    public class ViewExchangeRatesProfile : Profile
    {
        public ViewExchangeRatesProfile()
        {
            CreateMap<ExchangeRate, ExchangeRatesViewDTO>()
                .ForMember(x => x.CurrencyFrom, src => src.MapFrom(x => x.CurrencyFrom.Code))
                .ForMember(x => x.CurrencyTo, src => src.MapFrom(x => x.CurrencyTo.Code));
        }
    }
}
