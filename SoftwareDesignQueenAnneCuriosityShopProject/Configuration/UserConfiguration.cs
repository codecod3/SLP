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
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(u => u.UserID);
            builder.Property(u => u.UserID).ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName).HasColumnType("varchar(50)");

            builder.Property(u => u.LastName).HasColumnType("varchar(50)");

            builder.Property(u => u.PhoneNumber).HasColumnType("varchar(15)");

            builder.Property(u => u.Email).HasColumnType("varchar(100)");
             
            builder.Property(u => u.IsAdmin).HasColumnType("bit");
                
        }



    }
}
