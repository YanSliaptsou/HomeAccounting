using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAccounting.WebApi.DTOs.UserDto
{
    public class UserRequestDto
    {
        public string UserName { get; set; }
        public string MainCurrencyId { get; set; }
    }
}
