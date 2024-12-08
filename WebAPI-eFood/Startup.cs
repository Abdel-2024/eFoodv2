using eFood.Application.Mappings;
using eFood.Application.Validators;
using eFood.Infra.IoC;
using eFood.Infra.Storage.IServices;
using eFood.WebAPI.StorageServices;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebAPI_eFood
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistenceConfiguration(Configuration);

            services.AddAppsServicesConfiguration(Configuration);

            services.AddRepositoriesConfiguration(Configuration);

            services.AddAutoMapper(typeof(AutoMapperCreator));

            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            services.AddHttpContextAccessor();

            services.AddControllers().AddFluentValidation(
                option => option.RegisterValidatorsFromAssemblyContaining<ProductsCreateValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI_eFood", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI_eFood v1"));
            }

            app.UseStaticFiles();

            app.UseCors(o=>o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
