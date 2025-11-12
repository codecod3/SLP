using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace SoftwareDesignQueenAnneCuriosityShopProject.Configuration
{
    public class ClassAdviserConfiguration:IEntityTypeConfiguration<ClassAdviser>
    {
        public void Configure(EntityTypeBuilder<ClassAdviser> builder)
        {
            builder.ToTable(nameof(ClassAdviser));
            builder.HasKey(ca => ca.ClassAdviserID);
            builder.Property(ca => ca.ClassAdviserID).ValueGeneratedOnAdd();

            builder.Property(ca => ca.FirstName).HasColumnType("varchar(50)");

            builder.Property(ca => ca.LastName).HasColumnType("varchar(50)");

            builder.Property(ca => ca.PhoneNumber).HasColumnType("varchar(50");
                

            // Relationship with Advisory
            builder.HasMany(ca => ca.Advisories)
                .WithOne(a => a.ClassAdviserLink)
                .HasForeignKey(a => a.ClassAdviserID)
                .OnDelete(DeleteBehavior.Cascade);




        }
    }
}
