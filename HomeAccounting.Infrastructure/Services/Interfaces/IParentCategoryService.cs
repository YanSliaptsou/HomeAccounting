using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface IParentCategoryService
    {
        Task<bool> IsSuchParentCategoryExists(string userId, string parentCategoryName);
    }
}
