using project.Database.Configurations;
using project.Models;
using Microsoft.EntityFrameworkCore;

namespace project.Database
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Objects> Objects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectConfiguration());
        }

        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }
    }
}
