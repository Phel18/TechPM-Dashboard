using Microsoft.EntityFrameworkCore;
using TechPMDashboard.Models;

namespace TechPMDashboard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceAllocation> ResourceAllocations { get; set; }
        public DbSet<Risk> Risks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.ResourceAllocations)
                .WithOne(ra => ra.Project)
                .HasForeignKey(ra => ra.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Risks)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resource>()
                .HasMany(r => r.ProjectAllocations)
                .WithOne(ra => ra.Resource)
                .HasForeignKey(ra => ra.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resource>()
                .HasMany(r => r.AssignedTasks)
                .WithOne(t => t.AssignedTo)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Resources
            modelBuilder.Entity<Resource>().HasData(
                new Resource { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Role = "Senior Developer", AllocationPercentage = 115 },
                new Resource { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Role = "DevOps Engineer", AllocationPercentage = 85 },
                new Resource { Id = 3, FirstName = "Robert", LastName = "Johnson", Email = "robert.johnson@example.com", Role = "Network Administrator", AllocationPercentage = 60 },
                new Resource { Id = 4, FirstName = "Amanda", LastName = "Parker", Email = "amanda.parker@example.com", Role = "Database Administrator", AllocationPercentage = 40 }
            );

            // Seed Projects
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Name = "Infrastructure Upgrade", Status = ProjectStatus.OnTrack, ProgressPercentage = 65, StartDate = DateTime.Now.AddMonths(-2), EndDate = DateTime.Now.AddMonths(2), Description = "Upgrade company infrastructure" },
                new Project { Id = 2, Name = "Data Migration", Status = ProjectStatus.AtRisk, ProgressPercentage = 42, StartDate = DateTime.Now.AddMonths(-1), EndDate = DateTime.Now.AddMonths(3), Description = "Migrate data to new system" },
                new Project { Id = 3, Name = "Cloud Integration", Status = ProjectStatus.Delayed, ProgressPercentage = 28, StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(4), Description = "Integrate with cloud services" },
                new Project { Id = 4, Name = "Security Audit", Status = ProjectStatus.OnTrack, ProgressPercentage = 85, StartDate = DateTime.Now.AddMonths(-4), EndDate = DateTime.Now.AddDays(15), Description = "Conduct security audit" },
                new Project { Id = 5, Name = "Networking Setup", Status = ProjectStatus.OnTrack, ProgressPercentage = 72, StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(1), Description = "Setup new networking infrastructure" }
            );

            // Seed Tasks
            // Seed Tasks
            // Seed Tasks
            modelBuilder.Entity<ProjectTask>().HasData(
                new ProjectTask { Id = 1, Name = "Configure network switches", Description = "Install and configure new network switches in server room", Status = Models.TaskStatus.InProgress, ProjectId = 1, AssignedToId = 3, DueDate = DateTime.Now.AddDays(5) },
                new ProjectTask { Id = 2, Name = "Migrate user data to new system", Description = "Transfer all user profiles and data to the upgraded database", Status = Models.TaskStatus.Delayed, ProjectId = 2, AssignedToId = 4, DueDate = DateTime.Now.AddDays(-2) },
                new ProjectTask { Id = 3, Name = "Set up AWS security groups", Description = "Configure security groups and network ACLs for AWS deployment", Status = Models.TaskStatus.Completed, ProjectId = 3, AssignedToId = 2, DueDate = DateTime.Now.AddDays(-5) },
                new ProjectTask { Id = 4, Name = "Conduct penetration testing", Description = "Run security penetration tests against production environment", Status = Models.TaskStatus.Upcoming, ProjectId = 4, AssignedToId = 1, DueDate = DateTime.Now.AddDays(10) }
            );

            // Seed Resource Allocations
            modelBuilder.Entity<ResourceAllocation>().HasData(
                new ResourceAllocation { Id = 1, ProjectId = 1, ResourceId = 1, AllocationPercentage = 30 },
                new ResourceAllocation { Id = 2, ProjectId = 2, ResourceId = 1, AllocationPercentage = 40 },
                new ResourceAllocation { Id = 3, ProjectId = 3, ResourceId = 1, AllocationPercentage = 45 },
                new ResourceAllocation { Id = 4, ProjectId = 1, ResourceId = 2, AllocationPercentage = 25 },
                new ResourceAllocation { Id = 5, ProjectId = 3, ResourceId = 2, AllocationPercentage = 60 },
                new ResourceAllocation { Id = 6, ProjectId = 1, ResourceId = 3, AllocationPercentage = 60 },
                new ResourceAllocation { Id = 7, ProjectId = 2, ResourceId = 4, AllocationPercentage = 40 }
            );

            // Seed Risks
            modelBuilder.Entity<Risk>().HasData(
                new Risk { Id = 1, Description = "Database compatibility issues", Level = RiskLevel.High, ProjectId = 2, IsResolved = false, MitigationPlan = "Test in staging environment first" },
                new Risk { Id = 2, Description = "Security vulnerabilities in cloud setup", Level = RiskLevel.High, ProjectId = 3, IsResolved = false, MitigationPlan = "Perform security audit" },
                new Risk { Id = 3, Description = "Hardware delivery delays", Level = RiskLevel.Medium, ProjectId = 1, IsResolved = false, MitigationPlan = "Order from alternate vendor" },
                new Risk { Id = 4, Description = "Staff training availability", Level = RiskLevel.Low, ProjectId = 4, IsResolved = false, MitigationPlan = "Schedule training sessions in advance" }
            );
        }
    }
}



