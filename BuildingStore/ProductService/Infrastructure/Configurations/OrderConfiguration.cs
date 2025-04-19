using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;

namespace ProductService.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности заказа.
    /// </summary>
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.UserId)
                .IsRequired();
            builder.Property(o => o.Status)
                .IsRequired().HasConversion(
                x => x.ToString(), 
                x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));
            builder.Property(o => o.TotalPrice)
                .IsRequired()
                .HasPrecision(10, 2);
            builder.Property(o => o.CreatedAt)
                .IsRequired()
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasMany(o => o.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Reports)
                .WithOne(r => r.Order)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
