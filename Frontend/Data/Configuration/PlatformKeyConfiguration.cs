using Frontend.Common;
using Frontend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frontend.Data.Configuration
{
    public class PlatformKeyConfiguration : IEntityTypeConfiguration<PlatformKey>
    {
        public void Configure(EntityTypeBuilder<PlatformKey> builder)
        {
            builder.ToTable("PlatformKeys");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.UserId)
                   .IsRequired();

            builder.Property(p => p.Platform)
                   .IsRequired()
                   .HasConversion<string>(p => p.ToString(), p => (Platform)Enum.Parse(typeof(Platform), p));

            builder.Property(p => p.Secret)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.Public)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.PlatformKeys)
                   .HasForeignKey(p => p.UserId);
        }
    }
}
