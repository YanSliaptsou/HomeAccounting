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
            services.AddAutoMapper(typeof(CreateTransactionCategoryProfile).Assembly);
            services.AddAutoMapper(typeof(ViewTransactionCategoryProfile).Assembly);
            services.AddAutoMapper(typeof(ViewExchangeRatesProfile).Assembly);
            services.AddAutoMapper(typeof(UsersAccountsProfile).Assembly);
            services.AddCors();
            /*services.AddIdentityCore<AppUser>(opt =>
            {
                //opt.Password.RequireNonAlphanumeric = false;
            })
                .AddSignInManager<SignInManager<AppUser>>()
                .AddUserManager<UserManager<AppUser>>()
                .AddEntityFrameworkStores<DatabaseContext>();*/

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
