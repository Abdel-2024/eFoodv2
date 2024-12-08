using eFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Persistence.EntityConfigurations
{
    public class TypeOptionEntityConfig : IEntityTypeConfiguration<TypeOption>
    {
        public void Configure(EntityTypeBuilder<TypeOption> builder)
        {
            builder.HasKey(c => c.TypeOptionId);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(30);
            builder.Property(c => c.Description).HasMaxLength(100);

        }
    }
}
