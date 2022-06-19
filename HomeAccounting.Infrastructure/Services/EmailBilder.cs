using HomeAccounting.Domain.Enums;
using HomeAccounting.Domain.Models;
using HomeAccounting.Infrastructure.Helpers;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class EmailBilder : IEmailBilder
    {
        private readonly IEmailSender _emailSender;
        private readonly IHTMLTemplateService _htmlTemplateService;
        private const string EMAIL_CONFIRMATION_HEADER = "Email Confirmation";
        private const string RESET_PASSWORD_HEADER = "Reset password";
        private const string EMAIL_CONFIRMATION_TEMPLATE_NAME = "ConfEmailTempl";
        private const string PASSWORD_RESET_TEMPLATE_NAME = "ResetPassTempl";
        private const string LINK_REPLACE = "{link}";
        private const string USERNAME_REPLACE = "{username}";
        private const string EMAIL_REPLACE = "{email}";
        private const string PASSWORD_REPLACE = "{password}";

        public EmailBilder(IEmailSender emailSender, IHTMLTemplateService htmlTemplateService)
        {
            _emailSender = emailSender;
            _htmlTemplateService = htmlTemplateService;
        }

        public async Task GenerateEmailMessage(EmailMessageTemplate template, string link, AppUser appUser, string password = null)
        {
            var htmlTemplate = "";
            var messageHeader = "";
            switch(template)
            {
                case EmailMessageTemplate.ConfirmEmail:
                    {
                        messageHeader = EMAIL_CONFIRMATION_HEADER;
                        htmlTemplate = _htmlTemplateService.GetTemplate(EMAIL_CONFIRMATION_TEMPLATE_NAME).Result.Template
                            .Replace(LINK_REPLACE, link)
                            .Replace(USERNAME_REPLACE, appUser.UserName)
                            .Replace(EMAIL_REPLACE, appUser.Email)
                            .Replace(PASSWORD_REPLACE, password);
                        break;
                    }
                case EmailMessageTemplate.ResetPassword:
                    {
                        messageHeader = RESET_PASSWORD_HEADER;
                        htmlTemplate = _htmlTemplateService.GetTemplate(PASSWORD_RESET_TEMPLATE_NAME).Result.Template
                            .Replace(LINK_REPLACE, link)
                            .Replace(USERNAME_REPLACE, appUser.UserName);
                        break;
                    }
            }

            var message = new Message(new string[] { appUser.Email }, messageHeader, htmlTemplate);
            _emailSender.SendEmail(message);
        }
    }
}
