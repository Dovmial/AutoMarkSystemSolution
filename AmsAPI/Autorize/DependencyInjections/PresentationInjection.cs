using AmsAPI.Autorize.Data;
using AmsAPI.Autorize.Features;
using AmsAPI.Autorize.Interfaces;
using AmsAPI.Autorize.Repositories;
using AmsAPI.Autorize.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AmsAPI.Autorize.DependencyInjections
{
    public static class PresentationInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            AuthSettings authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>()!;
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<JwtTokenGenerator>();
            services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings))); //настройки внедряются через IOptions<AuthSettings> options

            services.AddDbContext<UserDbContext>(options => options.UseSqlite(configuration.GetConnectionString("UserDbConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey))
                    };

                    o.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[authSettings.CookieName];
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization();
            return services;
        }
    }
}
