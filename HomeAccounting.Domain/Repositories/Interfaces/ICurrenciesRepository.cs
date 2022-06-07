using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories.Interfaces
{
    public interface ICurrenciesRepository
    {
        Task<IEnumerable<Currency>> GetAllCurrencies();
    }
}
