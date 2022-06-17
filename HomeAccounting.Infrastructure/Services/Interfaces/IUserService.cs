using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetConcreteUser(string userId);
        Task EditUser(AppUser appUser, string userToEditId);
    }
}
