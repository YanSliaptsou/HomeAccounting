using HomeAccounting.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Db;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task EditUser(AppUser appUser, string userToEditId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userToEditId);
            if (user != null)
            {
                if (appUser.UserName != null)
                {
                    user.UserName = appUser.UserName;
                }

                if (appUser.MainCurrencyId != null)
                {
                    user.MainCurrencyId = appUser.MainCurrencyId;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<AppUser> GetConcreteUser(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<IEnumerable<AppUser>> GetUsersList()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
