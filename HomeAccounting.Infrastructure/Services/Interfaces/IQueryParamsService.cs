using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface IQueryParamsService
    {
        string BuildQueryString(string url, string token, string mail);
    }
}
