using Microsoft.EntityFrameworkCore;

namespace Project_Manage.Models
{
    public class ProjectContext:DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> op) : base(op)
        {

        }
        public DbSet<Project> projects { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Task> tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store enums as string
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Task>()
                .Property(t => t.Status)
                .HasConversion<string>();
        }
    }
}

