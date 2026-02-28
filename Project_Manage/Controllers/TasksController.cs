using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Manage.Models;

namespace Project_Manage.Controllers
{
    public class TasksController : Controller
    {
        private readonly ProjectContext _context;

        public TasksController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            var projectContext = _context.tasks.Include(t => t.Emp).Include(t => t.Pro);
            return View(await projectContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async System.Threading.Tasks.Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.tasks
                .Include(t => t.Emp)
                .Include(t => t.Pro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewData["Employee_id"] = new SelectList(_context.employees, "Id", "Id");
            ViewData["Project_id"] = new SelectList(_context.projects, "Id", "Id");

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Models.TaskStatus)));

            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Create([Bind("Id,Name,Project_id,Employee_id,Status")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();

                await UpdateProjectStatusAsync(task.Project_id);

                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee_id"] = new SelectList(_context.employees, "Id", "Id", task.Employee_id);
            ViewData["Project_id"] = new SelectList(_context.projects, "Id", "Id", task.Project_id);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async System.Threading.Tasks.Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["Employee_id"] = new SelectList(_context.employees, "Id", "Id", task.Employee_id);
            ViewData["Project_id"] = new SelectList(_context.projects, "Id", "Id", task.Project_id);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Models.TaskStatus)), task.Status);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Edit(int id, [Bind("Id,Name,Project_id,Employee_id,Status")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            var existingTask = await _context.tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTask == null)
            {
                return NotFound();
            }

            var previousProjectId = existingTask.Project_id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();

                    await UpdateProjectStatusAsync(previousProjectId);

                    if (previousProjectId != task.Project_id)
                    {
                        await UpdateProjectStatusAsync(task.Project_id);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee_id"] = new SelectList(_context.employees, "Id", "Id", task.Employee_id);
            ViewData["Project_id"] = new SelectList(_context.projects, "Id", "Id", task.Project_id);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async System.Threading.Tasks.Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.tasks
                .Include(t => t.Emp)
                .Include(t => t.Pro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetTasksByProject(int projectId)
        {
            var tasks = await _context.tasks
                .Where(t => t.Project_id == projectId)
                .Select(t => new {
                    id = t.Id,
                    name = t.Name,
                    status = t.Status.ToString()
                })
                .ToListAsync();

            return Json(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.tasks.FindAsync(id);
            if (task != null)
            {
                var projectId = task.Project_id;
                _context.tasks.Remove(task);
                await _context.SaveChangesAsync();
                await UpdateProjectStatusAsync(projectId);

                return RedirectToAction(nameof(Index));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async System.Threading.Tasks.Task UpdateProjectStatusAsync(int projectId)
        {
            var project = await _context.projects.FindAsync(projectId);
            if (project == null)
            {
                return;
            }

            var hasTasks = await _context.tasks.AnyAsync(t => t.Project_id == projectId);
            if (!hasTasks)
            {
                return;
            }

            var hasOpenTasks = await _context.tasks.AnyAsync(t => t.Project_id == projectId && t.Status != Models.TaskStatus.Complete);

            project.Status = hasOpenTasks ? ProjectStatus.Dev : ProjectStatus.Deployed;
            await _context.SaveChangesAsync();
        }

        private bool TaskExists(int id)
        {
            return _context.tasks.Any(e => e.Id == id);
        }
    }
}