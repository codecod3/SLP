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
    public class StudentConfiguration: IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {

            builder.ToTable(nameof(Student));
            builder.HasKey(s => s.StudentID);
            builder.Property(s => s.StudentID).ValueGeneratedOnAdd();

            builder.Property(s => s.FirstName).HasColumnType("varchar(50)");

            builder.Property(s => s.LastName).HasColumnType("varchar(50)");

            builder.Property(s => s.LRN).HasColumnType("int");

            builder.Property(s => s.EnrollmentStatus).HasColumnType("varchar(50)");

            builder.Property(s => s.PhoneNumber).HasColumnType("varchar(15)");

            builder.Property(s => s.UID).HasColumnType("varchar(100)");


            //Relationship
            builder.HasMany(s => s.Attendances)
                .WithOne(a => a.StudentLink)
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Cascade);

            


        }
    }
}
