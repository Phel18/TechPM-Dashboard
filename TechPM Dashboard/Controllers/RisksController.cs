using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechPMDashboard.Data;
using TechPMDashboard.Models;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace TechPMDashboard.Controllers
{
    public class RisksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RisksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRiskLevel(int id, RiskLevel level)
        {
            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return Json(new { success = false });
            }

            risk.Level = level;
            await _context.SaveChangesAsync();

            // Update project status if risk level is high
            if (level == RiskLevel.High)
            {
                await UpdateProjectStatus(risk.ProjectId);
            }

            return Json(new { success = true });
        }



        //[HttpGet]
        //public IActionResult Create()
        //{
        //    ViewBag.Projects = new SelectList(_context.Projects, "Id", "Name");
        //    return View();
        //}

        // GET: Risks/Create
        public IActionResult Create()
        {
            ViewBag.Projects = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: Risks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Level,ProjectId,MitigationPlan")] Risk risk)
        {
            if (ModelState.IsValid)
            {
                risk.IsResolved = false; // New risks are not resolved
                _context.Add(risk);
                await _context.SaveChangesAsync();

                // Update project status if this is a high-risk item
                if (risk.Level == RiskLevel.High)
                {
                    await UpdateProjectStatus(risk.ProjectId);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Projects = new SelectList(_context.Projects, "Id", "Name", risk.ProjectId);
            return View(risk);
        }
        //[HttpPost]
        //public async Task<IActionResult> Create([Bind("Description,Level,ProjectId")] Risk risk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(risk);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index", "Dashboard");
        //    }

        //    ViewBag.Projects = new SelectList(_context.Projects, "Id", "Name", risk.ProjectId);
        //    return View(risk);
        //}

        private async Task UpdateProjectStatus(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var highRisks = await _context.Risks
                .CountAsync(r => r.ProjectId == projectId && r.Level == RiskLevel.High);

            // If there are high risks, update project status to AtRisk
            if (highRisks > 0 && project.Status == ProjectStatus.OnTrack)
            {
                project.Status = ProjectStatus.AtRisk;
                await _context.SaveChangesAsync();
            }
        }
    }
}
