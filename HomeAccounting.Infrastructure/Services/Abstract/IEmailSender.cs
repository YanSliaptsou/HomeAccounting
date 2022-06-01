using HomeAccounting.Infrastructure.Helpers;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Abstract
{
    public interface IEmailSender
    {
        MimeMessage CreateEmailMessage(Message message);
        void SendEmail(Message message);
    }
}
