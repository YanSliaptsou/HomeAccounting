using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Domain.Db
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public override DbSet<AppUser> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<ParentTransactionCategory> ParentTransactionCategories { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<OutcomeLimit> OutcomeLimits { get; set; }
        public DbSet<HtmlSenderTemplate> HtmlSenderTemplates { get; set; }
    }
}
