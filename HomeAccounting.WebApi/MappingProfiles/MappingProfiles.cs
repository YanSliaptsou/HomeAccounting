using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.WebApi.DTOs.ParentCategoriesDTO;
using HomeAccounting.WebApi.DTOs.RegistrationDTOs;


namespace HomeAccounting.WebApi.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserRegistrationRequestDTO, AppUser>();
        }
    }
}
