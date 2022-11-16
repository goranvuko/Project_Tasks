using Microsoft.EntityFrameworkCore;
using Project_Tasks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Tasks.Data
{
    public class ProjectTasksDbContext : DbContext 
    {
        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<Project> Projects  { get; set; }

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=DESKTOP-0UCKG6H\SQLEXPRESS;Initial Catalog=ProjectTasksDb;Trusted_Connection=True;Integrated Security=True;Trust Server Certificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectID);
            modelBuilder.Entity<Project>().Property(t => t.Name).HasMaxLength(50);
            modelBuilder.Entity<Project>().Property(t => t.Code).HasMaxLength(50);
            modelBuilder.Entity<Entities.Task>().Property(t => t.Name).HasMaxLength(50);
            modelBuilder.Entity<Entities.Task>().Property(t => t.Description).HasMaxLength(50);
        }


    }
}
