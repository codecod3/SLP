using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject.Configuration;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;


namespace SoftwareDesignQueenAnneCuriosityShopProject
{
    public class Context: DbContext
    {
       
        public DbSet<Advisory> Advisories { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ClassAdviser> ClassAdvisers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Delivered> Delivereds { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\Jam\\Desktop\SLP - Copy\Project\SoftwareDesignQueenAnneCuriosityShopProject\SLPdatabase.db");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdvisoryConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
            modelBuilder.ApplyConfiguration(new ClassAdviserConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveredConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new ParentConfiguration());
            modelBuilder.ApplyConfiguration(new RelationshipConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
