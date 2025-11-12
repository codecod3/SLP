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
    public class NotificationConfiguration: IEntityTypeConfiguration<Notification>
    {

        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification));
            builder.HasKey(n => n.NotificationID);
            builder.Property(n => n.NotificationID).ValueGeneratedOnAdd();

            builder.Property(n => n.Message).HasColumnType("varchar(255)");
                
           

            // Relationship with User
            builder.HasOne(n => n.AttendanceLink)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.AttendanceID)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
