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
    public class AttendanceConfiguration: IEntityTypeConfiguration<Attendance>
    {

        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable(nameof(Attendance));
            builder.HasKey(a => a.AttendanceID);
            builder.Property(a => a.AttendanceID).ValueGeneratedOnAdd();

            builder.Property(a => a.Date).HasColumnType("datetime");
                

            // Relationship with User
            builder.HasOne(a => a.StudentLink)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
