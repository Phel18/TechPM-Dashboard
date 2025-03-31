using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechPMDashboard.Data;
using TechPMDashboard.Models;

namespace TechPMDashboard.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.DueDate)
                .ToListAsync();
            return View(tasks);
        }
        //public async Task<IActionResult> Index()
        //{
        //    var tasks = await _context.ProjectTasks  // Changed from Tasks to ProjectTasks
        //        .Include(t => t.Project)
        //        .OrderByDescending(t => t.DueDate)
        //        .ToListAsync();
        //    return View(tasks);
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(int id, TechPMDashboard.Models.TaskStatus status)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return Json(new { success = false });
            }

            task.Status = status;
            await _context.SaveChangesAsync();

            await UpdateProjectProgress(task.ProjectId);

            // Update project status based on delayed tasks
            await UpdateProjectStatusBasedOnTasks(task.ProjectId);

            return Json(new { success = true });


            //Update project progress if the task is completed
            //if (status == TechPMDashboard.Models.TaskStatus.Completed)
            //{
            //    await UpdateProjectProgress(task.ProjectId);
            //}

            //return Json(new { success = true });
        }



        private async Task UpdateProjectProgress(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();

            if (tasks.Any())
            {
                int completedTasks = tasks.Count(t => t.Status == TechPMDashboard.Models.TaskStatus.Completed);
                project.ProgressPercentage = (int)((double)completedTasks / tasks.Count * 100);
                await _context.SaveChangesAsync();
            }
        }

        private async Task UpdateProjectStatusBasedOnTasks(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();

            if (tasks.Any())
            {
                int delayedTasksCount = tasks.Count(t => t.Status == TechPMDashboard.Models.TaskStatus.Delayed);
                double delayedPercentage = (double)delayedTasksCount / tasks.Count;

                // Update project status based on delayed tasks
                if (delayedPercentage >= 0.3) // If 30% or more tasks are delayed
                {
                    project.Status = ProjectStatus.Delayed;
                }
                else if (delayedPercentage > 0) // If any tasks are delayed
                {
                    // Only change to AtRisk if not already Delayed
                    if (project.Status != ProjectStatus.Delayed)
                    {
                        project.Status = ProjectStatus.AtRisk;
                    }
                }
                // Don't automatically change back to OnTrack - let risk management handle that

                await _context.SaveChangesAsync();
            }
        }


    }
}
