using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using HomeAccounting.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Services
{
    public class HTMLTemplateService : IHTMLTemplateService
    {
        private readonly IHTMLTemplateRepository _hTMLTemplateRepository;

        public HTMLTemplateService(IHTMLTemplateRepository hTMLTemplateRepository)
        {
            _hTMLTemplateRepository = hTMLTemplateRepository;
        }

        public async Task<HtmlSenderTemplate> GetTemplate(string name)
        {
            return await _hTMLTemplateRepository.GetTemplate(name);
        }
    }
}
