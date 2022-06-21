using HomeAccounting.Infrastructure.Services;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.Infrastructure.Services.Concrete;
using HomeAccounting.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Infrastructure.Extensions
{
    public static class ServicesContainerExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IExchangeRatesService, ExchangeRatesService>();
            services.AddTransient<IParentCategoryService, ParentCategoryService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ILegderService, LegderService>();
            services.AddTransient<ILimitsService, LimitsService>();
            services.AddTransient<IRepCalculatorService, RepCalculatorService>();
            services.AddTransient<IRepConstructorService, RepConstructorService>();
            services.AddTransient<IRepItemsService, RepItemsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHTMLTemplateService, HTMLTemplateService>();
            services.AddTransient<IQueryParamsService, QueryParamsService>();
            services.AddTransient<IEmailBilder, EmailBilder>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}
