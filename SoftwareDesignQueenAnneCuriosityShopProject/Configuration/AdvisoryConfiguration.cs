using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Configuration
{
    internal class AdvisoryConfiguration: IEntityTypeConfiguration<Advisory>
    {
        public void Configure(EntityTypeBuilder<Advisory> builder)
        {
            builder.ToTable(nameof(Advisory));
            builder.HasKey(a => a.AdvisoryID);
            builder.Property(a => a.AdvisoryID).ValueGeneratedOnAdd();

            builder.Property(a => a.Name).HasColumnType("varchar(100)");
                


            builder.Property(a => a.SectionName).HasColumnType("varchar(50)");
                


            builder.Property(a => a.SchoolYear).HasColumnType("varchar(9)");
                
               

            // Relationship with ClassAdviser
            builder.HasOne(a => a.ClassAdviserLink)
                .WithMany(ca => ca.Advisories)
                .HasForeignKey(a => a.ClassAdviserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Students
            builder.HasMany(a => a.Students)
                .WithOne(s => s.AdvisoryLink)
                .HasForeignKey(s => s.AdvisoryID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
