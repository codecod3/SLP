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
    public class ParentConfiguration: IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable(nameof(Parent));
            builder.HasKey(p => p.ParentID);
            builder.Property(p => p.ParentID).ValueGeneratedOnAdd();

            builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
                
            builder.Property(p => p.LastName).HasColumnType("varchar(50");
                
            builder.Property(p => p.Email).HasColumnType("varchar(50)");
                
                

            // Relationship with Contact
            builder.HasMany(p => p.Contacts)
                .WithOne(c => c.ParentLink)
                .HasForeignKey(c => c.ParentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Relationship
            builder.HasMany(p => p.Relationships)
                .WithOne(r => r.ParentLink)
                .HasForeignKey(r => r.ParentID)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
