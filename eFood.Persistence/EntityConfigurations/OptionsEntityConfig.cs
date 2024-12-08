using eFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFood.Persistence.EntityConfiguration
{
    public class OptionsEntityConfig : IEntityTypeConfiguration<Options>
    {
        public void Configure(EntityTypeBuilder<Options> builder)
        {
            builder.HasKey(o=>o.OptionId);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(30);
            builder.Property(o => o.Description).HasMaxLength(100);

            builder.HasOne(c=>c.TypeOption)
                .WithMany(o=>o.Options)
                .HasForeignKey(o=>o.TypeOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull);   
        }
    }
}
