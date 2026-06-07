using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioTracker.Core.Models;

namespace PortfolioTracker.Core.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasMany(e => e.PlatformKeys)
                   .WithOne()
                   .HasForeignKey(p => p.UserId);
        }
    }
}
