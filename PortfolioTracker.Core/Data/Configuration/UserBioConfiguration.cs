using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Core.Data.Configuration
{ 
    public class UserBioConfiguration : IEntityTypeConfiguration<UserBio>
    {
        public void Configure(EntityTypeBuilder<UserBio> builder)
        {
            builder.ToTable("UserBio");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User)
                   .WithOne()
                   .HasForeignKey<UserBio>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
