using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Configurations
{
    /// <summary>
    /// Конфигурация сущности отчета.
    /// </summary>
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.ReportId);

            builder.Property(r => r.SellerId)
                .IsRequired();
            builder.Property(r => r.OrderId)
                .IsRequired();
            builder.Property(r => r.CreatedAt)
                .IsRequired()
                 .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(r => r.Amount)
                .IsRequired();
            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(r => r.Order)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
