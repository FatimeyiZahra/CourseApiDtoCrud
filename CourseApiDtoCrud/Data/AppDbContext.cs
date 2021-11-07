using CourseApiDtoCrud.Data.Configurations;
using CourseApiDtoCrud.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApiDtoCrud.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<AppUser> AppUsers { get; set; }
        //public DbSet<Tag> Tags { get; set; }
        //public DbSet<CourseTag> CourseTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            //modelBuilder.ApplyConfiguration(new TagConfiguration());
            //modelBuilder.ApplyConfiguration(new CourseTagConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
