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
    internal class RelationshipConfiguration: IEntityTypeConfiguration<Relationship>
    {

        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            builder.ToTable(nameof(Relationship));
            builder.HasKey(r => r.RelationshipID);
            builder.Property(r => r.RelationshipID).ValueGeneratedOnAdd();

            builder.Property(r => r.TypeOfRelationship).HasColumnType("varchar(50)");
               
                

            // Relationship with Parent
            builder.HasOne(r => r.ParentLink)
                .WithMany(p => p.Relationships)
                .HasForeignKey(r => r.ParentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Student
            builder.HasOne(r => r.StudentLink)
                .WithMany(s => s.Relationships)
                .HasForeignKey(r => r.StudentID)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
