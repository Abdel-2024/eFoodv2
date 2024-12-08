using eFood.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eFood.Infra.IoC
{
    public static class PersistenceConfiguration
    {
        public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<eFoodContext>(option =>
                                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                b => b.MigrationsAssembly(typeof(eFoodContext).Assembly.FullName)));

            return services;    
        }
    }
}
