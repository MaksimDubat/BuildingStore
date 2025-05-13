using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName)
                .IsRequired();
            builder.Property(x => x.UserEmail)
                .IsRequired();
            builder.Property(x => x.PasswordHash)
                .IsRequired();
            builder.Property(x => x.Role)
                .IsRequired().HasConversion(
                x => x.ToString(),
                x => (UserRole)Enum.Parse(typeof(UserRole), x));

        }
    }
}
