using Microsoft.EntityFrameworkCore;
using System;
using TaskTracker.Database.Entities;

namespace TaskTracker.Database
{
    public class TaskTrackerContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public TaskTrackerContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-QC7UKQR;Database=TaskTrackerDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne(p => p.Project)
                .WithMany(b => b.Tasks);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(b => b.Project);
        }
    }
}
