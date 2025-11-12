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
    internal class ContactConfiguration: IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable(nameof(Contact));
            builder.HasKey(c => c.ContactID);
            builder.Property(c => c.ContactID).ValueGeneratedOnAdd();

            builder.Property(c => c.PhoneNumber).HasColumnType("varchar(50)");



            builder.Property(c => c.Network).HasColumnType("varchar(50)");
                

            // Relationship
            builder.HasOne(c => c.ParentLink)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.ParentID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(c => c.DeliveredMsg)
                .WithOne(d => d.ContactLink)
                .HasForeignKey(d => d.ContactID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
