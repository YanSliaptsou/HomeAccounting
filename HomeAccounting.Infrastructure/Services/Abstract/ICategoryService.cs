using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface ICategoryService
    {
        Task<bool> IsSuchCategoryExists(string userId, string categoryName);
    }
}
