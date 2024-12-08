using eFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFood.Persistence.EntityConfiguration
{
    public class ProductsOptionsEntityConfig : IEntityTypeConfiguration<ProductsOptions>
    {
        public void Configure(EntityTypeBuilder<ProductsOptions> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.OptionId});

            builder.HasOne(o => o.Options)
                .WithMany(po => po.ProductsOptions)
                .HasForeignKey(o => o.OptionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(p => p.Products)
                .WithMany(po => po.ProductsOptions)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
