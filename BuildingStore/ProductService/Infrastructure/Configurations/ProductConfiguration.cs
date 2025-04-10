using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности продукта.
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(p => p.Price)
                .HasPrecision(10, 2)
                .IsRequired();
            builder.Property(p => p.ImageURL)
                .IsRequired();
            builder.Property(p => p.CreatedAt)
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(p =>p.UpdatedAt)
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(p => p.SaleCode); 
            builder.Property(p => p.SaleEndDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(p => p.SalePrice)
                .HasPrecision(10, 2);

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.OrderItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
