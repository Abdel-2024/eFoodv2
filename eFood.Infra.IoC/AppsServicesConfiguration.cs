using eFood.Application.IServices;
using eFood.Application.Services;
using eFood.Infra.Storage.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eFood.Infra.IoC
{
    public static class AppsServicesConfiguration
    {
        public static IServiceCollection AddAppsServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryProductService, CategoryProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITypeOptionService, TypeOptionService>();
            services.AddScoped<IOptionsService, OptionsService>();
            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<IJsonSerializer, JsonSerializer>();

            return services;
        }
    }
}
