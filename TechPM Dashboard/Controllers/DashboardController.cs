using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechPMDashboard.Data;
using TechPMDashboard.Models;
using TechPMDashboard.ViewModels;

namespace TechPMDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                Projects = await _context.Projects.ToListAsync(),
                RecentTasks = await _context.Tasks
                    .Include(t => t.Project)
                    .OrderByDescending(t => t.Id)
                    .Take(5)
                    .ToListAsync(),
                Resources = await _context.Resources
                    .ToListAsync(),
                Risks = await _context.Risks
                    .Include(r => r.Project)
                    .OrderByDescending(r => r.Level)
                    .Take(5)
                    .ToListAsync(),
                ActiveProjectsCount = await _context.Projects.CountAsync(),
                PendingTasksCount = await _context.Tasks
                    .CountAsync(t => t.Status != TechPMDashboard.Models.TaskStatus.Completed),
                TeamMembersCount = await _context.Resources.CountAsync(),
                HighPriorityRisksCount = await _context.Risks
                    .CountAsync(r => r.Level == RiskLevel.High),
                OnTrackProjectsCount = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.OnTrack),
                AtRiskProjectsCount = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.AtRisk),
                DelayedProjectsCount = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.Delayed)
            };

            // Calculate allocation percentages after loading resources
            foreach (var resource in viewModel.Resources)
            {
                // Get the sum of allocations for this resource
                resource.AllocationPercentage = await _context.ResourceAllocations
                    .Where(ra => ra.ResourceId == resource.Id)
                    .SumAsync(ra => ra.AllocationPercentage);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> GetProjectStatusData()
        {
            var onTrackCount = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.OnTrack);
            var atRiskCount = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.AtRisk);
            var delayedCount = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.Delayed);

            return Json(new
            {
                onTrack = onTrackCount,
                atRisk = atRiskCount,
                delayed = delayedCount
            });
        }
    }
}



//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;
//using TechPMDashboard.Data;
//using TechPMDashboard.Models;
//using TechPMDashboard.ViewModels;

//namespace TechPMDashboard.Controllers
//{
//    public class DashboardController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public DashboardController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var viewModel = new DashboardViewModel
//            {
//                Projects = await _context.Projects.ToListAsync(),
//                RecentTasks = await _context.Tasks
//                    .Include(t => t.Project)
//                    .OrderByDescending(t => t.Id)
//                    .Take(5)
//                    .ToListAsync(),
//                Resources = await _context.Resources
//                    .ToListAsync(),  // Just get the resources directly
//                Risks = await _context.Risks
//                    .Include(r => r.Project)
//                    .OrderByDescending(r => r.Level)
//                    .Take(5)
//                    .ToListAsync(),
//                ActiveProjectsCount = await _context.Projects.CountAsync(),
//                PendingTasksCount = await _context.Tasks
//                    .CountAsync(t => t.Status != TechPMDashboard.Models.TaskStatus.Completed),
//                TeamMembersCount = await _context.Resources.CountAsync(),
//                HighPriorityRisksCount = await _context.Risks
//                    .CountAsync(r => r.Level == RiskLevel.High)
//            };

//            // Calculate allocation percentages after loading resources
//            foreach (var resource in viewModel.Resources)
//            {
//                // Get the sum of allocations for this resource
//                resource.AllocationPercentage = await _context.ResourceAllocations
//                    .Where(ra => ra.ResourceId == resource.Id)
//                    .SumAsync(ra => ra.AllocationPercentage);
//            }

//            return View(viewModel);
//        }



//    }
//}