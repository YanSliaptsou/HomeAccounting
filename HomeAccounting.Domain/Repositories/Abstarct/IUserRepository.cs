using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Abstarct
{
    public interface IUserRepository
    {
        Task ChangeMainCurrency(string userId, string currencyCode);
    }
}
