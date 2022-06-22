using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task EditUser(AppUser appUser, string userToEditId)
        {
            await _userRepository.EditUser(appUser, userToEditId);
        }

        public async Task<AppUser> GetConcreteUser(string userId)
        {
            return await _userRepository.GetConcreteUser(userId);
        }
    }
}
