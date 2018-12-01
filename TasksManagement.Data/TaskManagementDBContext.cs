using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using TasksManagement.Data.Entities;

namespace TasksManagement.Data
{
    public class TaskManagementDBContext : IdentityDbContext<User>
    {
        public DbSet<Task> Tasks { get; set; }

        public TaskManagementDBContext(DbContextOptions<TaskManagementDBContext> options) : base(options) { }

        public TaskManagementDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration["ConnectionStrings:TasksDatabase"];
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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


            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Name = "Support", NormalizedName = "Support".ToUpper() }
            );

        }
    }
}
