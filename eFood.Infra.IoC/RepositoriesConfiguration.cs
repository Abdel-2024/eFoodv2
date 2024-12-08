using eFood.Domain.IRepositories;
using eFood.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Infra.IoC
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositoriesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
