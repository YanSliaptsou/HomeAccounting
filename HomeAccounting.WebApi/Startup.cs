using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Domain.Repositories.Abstarct;
using HomeAccounting.Domain.Repositories.Concrete;
using HomeAccounting.Domain.Db;
using HomeAccounting.Domain.MappingProfiles;
using HomeAccounting.Domain.Models;
using Microsoft.AspNetCore.Identity;
using HomeAccounting.WebApi.MappingProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HomeAccounting.Infrastructure.Services.Abstract;
using HomeAccounting.Infrastructure.Services.Concrete;
using HomeAccounting.Infrastructure.Configurations;

namespace HomeAccounting.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = Configuration.GetSection("JwtSettings");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeAccounting.WebApi", Version = "v1" });
            });

            services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExchangeRatesRepository, ExhangeRatesRepository>();
            services.AddTransient<ITransactionCategoryRepository, TransactionCategoryRepository>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IExchangeRatesService, ExchangeRatesService>();
            services.AddAutoMapper(typeof(CreateTransactionCategoryProfile).Assembly);
            services.AddAutoMapper(typeof(ViewTransactionCategoryProfile).Assembly);
            services.AddAutoMapper(typeof(ViewExchangeRatesProfile).Assembly);
            services.AddAutoMapper(typeof(UsersAccountsProfile).Assembly);
            services.AddCors();

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeAccounting.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
