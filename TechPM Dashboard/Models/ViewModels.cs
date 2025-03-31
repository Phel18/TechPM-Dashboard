using System.Collections.Generic;
using TechPMDashboard.Models;

namespace TechPMDashboard.ViewModels
{
    public class DashboardViewModel
    {
        public int ActiveProjectsCount { get; set; }
        public int PendingTasksCount { get; set; }
        public int TeamMembersCount { get; set; }
        public int HighPriorityRisksCount { get; set; }

        public List<Project> Projects { get; set; }
        public List<ProjectTask> RecentTasks { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Risk> Risks { get; set; }

        public int OnTrackProjectsCount { get; set; }
        public int AtRiskProjectsCount { get; set; }
        public int DelayedProjectsCount { get; set; }
    }
}
