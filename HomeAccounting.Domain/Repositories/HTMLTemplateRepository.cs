using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Repositories
{
    public class HTMLTemplateRepository : IHTMLTemplateRepository
    {
        private readonly DatabaseContext _databaseContext;

        public HTMLTemplateRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<HtmlSenderTemplate> GetTemplate(string name)
        {
            return await _databaseContext.HtmlSenderTemplates.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
