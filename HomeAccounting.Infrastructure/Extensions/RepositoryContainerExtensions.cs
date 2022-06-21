using HomeAccounting.Domain.Repositories;
using HomeAccounting.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Extensions
{
    public static class RepositoryContainerExtensions
    {
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExchangeRatesRepository, ExhangeRatesRepository>();
            services.AddTransient<ITransactionCategoryRepository, TransactionCategoryRepository>();

            services.AddTransient<ICurrenciesRepository, CurrenciesRepository>();
            services.AddTransient<IParentTransactionCategoryRepository, ParentTransactionCategoryRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ILimitsRepository, LimitsRepository>();
            services.AddTransient<ILegderRepository, LegderRepository>();
            services.AddTransient<IHTMLTemplateRepository, HTMLTemplateRepository>();
            return services;
        }
    }
}
