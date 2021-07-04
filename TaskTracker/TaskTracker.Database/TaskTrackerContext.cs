using Microsoft.EntityFrameworkCore;
using System;
using TaskTracker.Database.Entities;

namespace TaskTracker.Database
{
    public class TaskTrackerContext : DbContext
    {
        public DbSet<Task> Students { get; set; }
        public DbSet<Project> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-QC7UKQR;Database=TaskTrackerDb;Trusted_Connection=True;");
        }
    }
}
