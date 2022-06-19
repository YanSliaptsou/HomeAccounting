using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class QueryParamsService : IQueryParamsService
    {
        private const string TOKEN_LABEL = "?token=";
        private const string EMAIL_LABEL = "&email=";
        public string BuildQueryString(string url, string token, string mail)
        {
            return url + TOKEN_LABEL + token + EMAIL_LABEL + mail;
        }
    }
}
