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
    public class DeliveredConfiguration: IEntityTypeConfiguration<Delivered>
    {
        public void Configure(EntityTypeBuilder<Delivered> builder)
        {
            builder.ToTable(nameof(Delivered));
            builder.HasKey(d => d.DeliveredID);
            builder.Property(d => d.DeliveredID).ValueGeneratedOnAdd();

            builder.Property(d => d.DateTimeSent).HasColumnType("datetime");
                

            // Relationship
            builder.HasOne(c=>c.ContactLink)
                .WithMany(o => o.DeliveredMsg)
                .HasForeignKey(d => d.ContactID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.NotificationLink)
                .WithMany(o => o.DeliveredMsg)
                .HasForeignKey(d => d.NotificationID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
