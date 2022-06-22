using HomeAccounting.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services.Interfaces
{
    public interface IHTMLTemplateService
    {
        Task<HtmlSenderTemplate> GetTemplate(string name);
    }
}
