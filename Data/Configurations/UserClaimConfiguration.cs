
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleJwtApi.Models;

namespace SampleJwtApi.Data.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasKey(uc => uc.Id);
            builder.Property(uc => uc.ClaimType).IsRequired().HasMaxLength(100);
            builder.Property(uc => uc.ClaimValue).IsRequired().HasMaxLength(100);

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId);
        }
    }
}
