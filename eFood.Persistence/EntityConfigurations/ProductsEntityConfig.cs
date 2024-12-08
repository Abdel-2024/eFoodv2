using eFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFood.Persistence.EntityConfiguration
{
    public class ProductsEntityConfig : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(45);
            builder.Property(p => p.Description).HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasPrecision(10, 2);
            builder.Property(p => p.IsNew).HasDefaultValue(true);
            builder.Property(p => p.IsRecommended).HasDefaultValue(false);

            builder.HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
