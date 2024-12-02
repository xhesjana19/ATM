using System.Globalization;
using AutoMapper;
using FluentValidation.AspNetCore;
using ATM.Core;
using ATM.Core.Services.Authentication;
using ATM.Web.Infrastructure;
using ATM.Web.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using ATM.Core.Services.Cryptography;
using ATM.Core.Services.Users;
using ATM.Core.Services.UsersAccount;
using ATM.Core.Services.ATMs;
using ATM.Core.Services.RechargeATM;
using ATM.Core.Services.ATMWithdrawals;

namespace ATM.Web
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
            // lowercase url
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            // .net services
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.ConfigureAuthentication(Configuration);

            // add context
            services.AddATMData(Configuration.GetConnectionString("DefaultConnection"), "ATM.Core");
            //auto mapper
            services.AddAutoMapper(expression => expression.AddMaps(typeof(BaseEntity).Assembly));

            // add services
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IATMService, ATMService>();
            services.AddScoped<IRechargeService, RechargeService>();
            services.AddScoped<IATMWithdrawalsService, ATMWithdrawalsService>();

            // domain services
            services.AddScoped<IUserService, UserService>();
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining(typeof(Startup));
                options.ImplicitlyValidateChildProperties = true;
                options.RegisterValidatorsFromAssemblyContaining<UserModel>();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
