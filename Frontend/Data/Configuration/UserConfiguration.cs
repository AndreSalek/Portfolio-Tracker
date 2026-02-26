using Frontend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frontend.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
     
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

            builder.Property(a => a.Username)
                    .IsRequired()
                    .HasMaxLength(200);

            builder.Property(a => a.Password)
                    .IsRequired()
                    .HasMaxLength(200);
        }
    }
}
