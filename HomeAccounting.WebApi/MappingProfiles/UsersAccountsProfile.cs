using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.WebApi.DTOs.RegistrationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.MappingProfiles
{
    public class UsersAccountsProfile : Profile
    {
        public UsersAccountsProfile()
        {
            CreateMap<UserRegistrationRequestDTO, AppUser>();
        }
    }
}
