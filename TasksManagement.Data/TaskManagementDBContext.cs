using Microsoft.EntityFrameworkCore;
using System;
using TasksManagement.Core.Entities;

namespace TasksManagement.Data
{
    public class TaskManagementDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Role> Roles { get; set; }

        public TaskManagementDBContext(DbContextOptions options) : base(options) { }

        public TaskManagementDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);

            optionsBuilder.UseSqlServer(@"Data Source=KS-SQL2014;Initial Catalog=TasksDB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.ID);

                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.RoleID).IsRequired();

                entity.HasOne(u => u.Role).WithMany(r => r.Users);
                entity.HasMany(u => u.OwnedTasks).WithOne(t => t.OwnerUser).HasForeignKey(p => p.OwnerUserID).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.AssignedTasks).WithOne(t => t.AssignedToUser).HasForeignKey(p => p.AssignedToUserID).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.ID);

                entity.Property(r => r.Name).IsRequired();

                entity.HasMany(r => r.Users).WithOne(u => u.Role).HasForeignKey(p => p.RoleID);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(t => t.ID);

                entity.Property(t => t.Title).IsRequired();
                entity.Property(t => t.Description);
                entity.Property(t => t.Created).IsRequired();
                entity.Property(t => t.OwnerUserID).IsRequired();
                entity.Property(t => t.AssignedToUserID).IsRequired(false);


                entity.HasOne(t => t.OwnerUser).WithMany(u => u.OwnedTasks).HasForeignKey(p => p.OwnerUserID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.AssignedToUser).WithMany(u => u.AssignedTasks).HasForeignKey(p => p.AssignedToUserID).OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<Role>().HasData(
                new Role { ID = 1, Name = "Admin" },
                new Role { ID = 2, Name = "Support" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { ID = 1, Name = "Admin1", RoleID = 1 },
                new User { ID = 2, Name = "Support1", RoleID = 2 },
                new User { ID = 3, Name = "Support2", RoleID = 2 }
            );

            modelBuilder.Entity<Task>().HasData(
                new Task { ID = 1, Title = "Task1", Description = "Task 1 Description", Created = DateTime.Now, OwnerUserID = 2 },
                new Task { ID = 2, Title = "Task2", Description = "Task 2 Description", Created = DateTime.Now, OwnerUserID = 3 }
            );
        }
    }
}
