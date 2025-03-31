using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechPMDashboard.Data;
using TechPMDashboard.Models;

namespace TechPMDashboard.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var resources = await _context.Resources
                .Include(r => r.ProjectAllocations)  // Changed from ResourceAllocations to ProjectAllocations
                .ThenInclude(ra => ra.Project)
                .ToListAsync();

            foreach (var resource in resources)
            {
                // If AllocationPercentage isn't automatically calculated in the model
                int totalAllocation = 0;
                if (resource.ProjectAllocations != null)
                {
                    totalAllocation = resource.ProjectAllocations.Sum(pa => pa.AllocationPercentage);
                }
                resource.AllocationPercentage = totalAllocation;
            }

            return View(resources);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAllocation(int resourceId, int projectId, int allocation)
        {
            // Check if allocation already exists
            var existingAllocation = await _context.ResourceAllocations
                .FirstOrDefaultAsync(ra => ra.ResourceId == resourceId && ra.ProjectId == projectId);

            if (existingAllocation != null)
            {
                existingAllocation.AllocationPercentage = allocation;
            }
            else
            {
                _context.ResourceAllocations.Add(new ResourceAllocation
                {
                    ResourceId = resourceId,
                    ProjectId = projectId,
                    AllocationPercentage = allocation
                });
            }

            await _context.SaveChangesAsync();

            // Return a redirect to the dashboard instead of a Json result
            return RedirectToAction("Index", "Dashboard");
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateAllocation(int resourceId, int projectId, int allocation)
        //{
        //    // Check if allocation already exists
        //    var existingAllocation = await _context.ResourceAllocations
        //        .FirstOrDefaultAsync(ra => ra.ResourceId == resourceId && ra.ProjectId == projectId);

        //    if (existingAllocation != null)
        //    {
        //        existingAllocation.AllocationPercentage = allocation;
        //    }
        //    else
        //    {
        //        _context.ResourceAllocations.Add(new ResourceAllocation
        //        {
        //            ResourceId = resourceId,
        //            ProjectId = projectId,
        //            AllocationPercentage = allocation
        //        });
        //    }

        //    await _context.SaveChangesAsync();
        //    return Json(new { success = true });
        //}
    }
}
