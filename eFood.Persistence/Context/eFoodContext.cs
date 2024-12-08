using eFood.Domain.Entities;
using eFood.Persistence.EntityConfiguration;
using eFood.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace eFood.Persistence.Context
{
    public class eFoodContext : DbContext
    {
        public eFoodContext(DbContextOptions<eFoodContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<TypeOption> TypeOption { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductsOptions> ProductsOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TypeOptionEntityConfig());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfig());
            modelBuilder.ApplyConfiguration(new OptionsEntityConfig());
            modelBuilder.ApplyConfiguration(new ProductsEntityConfig());
            modelBuilder.ApplyConfiguration(new ProductsOptionsEntityConfig());

            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
            //        t => t.GetInterfaces().Any(gi => gi.IsGenericType 
            //        && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        }
    }
}
