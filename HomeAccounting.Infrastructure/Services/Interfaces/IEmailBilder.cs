using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public interface IEmailBilder
    {
        Task GenerateEmailMessage(EmailMessageTemplate template, string link, AppUser appUser, string password = null);
    }
}
