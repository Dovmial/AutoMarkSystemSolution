
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ArmDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IMarkingCodeDAL, ArmMarkingCodeRepository>();
            services.AddScoped<IProductDAL, ProductRepository>();
            services.AddScoped<ISessionDAL, SessionRepository>();
            services.AddScoped<IProductionLineDAL, ProductionLineRepository>();
            return services;
        }

    }
}
