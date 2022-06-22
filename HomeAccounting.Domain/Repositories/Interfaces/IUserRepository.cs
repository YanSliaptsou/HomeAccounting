using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsersList();
        Task<AppUser> GetConcreteUser(string userId);
        Task EditUser(AppUser appUser, string userToEditId);
    }
}
