using System.Security.Claims;
using ATM.Core;
using ATM.Core.Data;
using ATM.Core.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATM.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = AuthenticationSettings.AuthenticationScheme;
                options.DefaultSignInScheme = AuthenticationSettings.ExternalAuthenticationScheme;
            });

            //add main cookie authentication
            authenticationBuilder.AddCookie(AuthenticationSettings.AuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{AuthenticationSettings.CookiePrefix}{AuthenticationSettings.CookieAuthentication}";
                options.Cookie.HttpOnly = true;
                options.LoginPath = new PathString(AuthenticationSettings.LoginPath);
                options.AccessDeniedPath = new PathString(AuthenticationSettings.AccessDeniedPath);
                options.LogoutPath = new PathString(AuthenticationSettings.LogoutPath);

                //whether to allow the use of authentication cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });


            //add external authentication
            authenticationBuilder.AddCookie(AuthenticationSettings.ExternalAuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{AuthenticationSettings.CookiePrefix}{AuthenticationSettings.CookieAuthentication}";
                options.Cookie.HttpOnly = true;
                options.LoginPath = AuthenticationSettings.LoginPath;
                options.AccessDeniedPath = AuthenticationSettings.AccessDeniedPath;

                //whether to allow the use of authentication cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.Administrator,
                    policy => policy.RequireClaim(ClaimTypes.Role, Roles.Administrator));
                
                options.AddPolicy(Roles.User,
                    policy => policy.RequireClaim(ClaimTypes.Role, Roles.User));
            });
        }

        /// <summary>
        /// Adds sql server with repositories to the application.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="migrationAssembly">Migration assembly.</param>
        public static IServiceCollection AddATMData(this IServiceCollection services, string connectionString, string migrationAssembly)
        {
            // repositories
            services.AddScoped(typeof(IRepository<>), typeof(ATMRepository<>));

            // database
            services.AddScoped<IDbContext, ATMDbContext>();

            // add context
            services.AddSqlServer(connectionString, migrationAssembly);

            return services;
        }


        #region Private Helpers

        /// <summary>
        /// Adds sql server to the application.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="migrationAssembly">Migration assembly.</param>
        private static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionString, string migrationAssembly)
        {
            return services.AddDbContext<ATMDbContext>(optionsBuilder =>
            {
                var dbContextOptionsBuilder = optionsBuilder.UseLazyLoadingProxies();
                dbContextOptionsBuilder.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(migrationAssembly));
                dbContextOptionsBuilder.EnableSensitiveDataLogging();
            });
        }

        #endregion
    }
}