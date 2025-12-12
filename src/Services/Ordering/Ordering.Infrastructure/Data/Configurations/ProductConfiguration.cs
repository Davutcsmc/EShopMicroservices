using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property( p=> p.Id ).HasConversion(
                productId => productId.Value,
                dbId => ProductId.Of(dbId)
            );

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}
