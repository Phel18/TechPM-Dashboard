using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechPMDashboard.Models
{
    public enum ProjectStatus
    {
        OnTrack,
        AtRisk,
        Delayed
    }

    public enum TaskStatus
    {
        Upcoming,
        InProgress,
        Completed,
        Delayed
    }

    public enum RiskLevel
    {
        Low,
        Medium,
        High
    }

    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        [Range(0, 100)]
        public int ProgressPercentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<ProjectTask> Tasks { get; set; }
        public virtual ICollection<ResourceAllocation> ResourceAllocations { get; set; }
        public virtual ICollection<Risk> Risks { get; set; }
    }

    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public TaskStatus Status { get; set; }

        public DateTime DueDate { get; set; }
        public string Description { get; set; }

        // Foreign keys
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual Resource AssignedTo { get; set; }
    }

    public class Resource
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }
        public int AllocationPercentage { get; set; }
        public bool IsOverallocated => AllocationPercentage > 100;

        // Navigation properties
        public virtual ICollection<ResourceAllocation> ProjectAllocations { get; set; }
        public virtual ICollection<ProjectTask> AssignedTasks { get; set; }

        // Helper properties
        public string FullName => $"{FirstName} {LastName}";
        public string Initials => $"{FirstName[0]}{LastName[0]}";
    }

    public class ResourceAllocation
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ResourceId { get; set; }
        public int AllocationPercentage { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual Resource Resource { get; set; }
    }

    public class Risk
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public RiskLevel Level { get; set; }

        public string MitigationPlan { get; set; }
        public bool IsResolved { get; set; }

        // Foreign key
        public int ProjectId { get; set; }

        // Navigation property
        public virtual Project Project { get; set; }
    }
}


