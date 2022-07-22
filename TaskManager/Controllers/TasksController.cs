using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Request;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TasksController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
                
        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmpTasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Epics)
                .Include(t => t.Projects)
                .Include(t => t.RequestedUser)
                .Include(t => t.User)
                .Include(t => t.OptionType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmpTasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.EmpTasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Epics)
                .Include(t => t.Projects)
                .Include(t => t.RequestedUser)
                .Include(t => t.User)
                .Include(t => t.OptionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // GET: Tasks/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");

            /*ViewData["PriorityID"] = new SelectList(_context.OptionTypes, "Id", "OptionName");*/
            ViewData["PriorityID"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Priority"), "Id", "OptionName");
            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName");
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "EpicsName");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName");
            ViewData["RequestedBy"] = new SelectList(managerList, "Id", "FirstName");
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TasksRequest taskrequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ETasks tasks = new ETasks();
            if (ModelState.IsValid)
            {
                tasks.TaskName = taskrequest.TaskName;
                tasks.PriorityID = taskrequest.PriorityID;
                tasks.ProjectID = taskrequest.ProjectID;
                tasks.DueDate = taskrequest.DueDate;
                tasks.EpicsID = taskrequest.EpicsID;
                tasks.EstTime = taskrequest.EstTime;
                tasks.AssignedTo = taskrequest.AssignedTo;
                tasks.RequestedBy = taskrequest.RequestedBy;
                tasks.RequestDate = taskrequest.RequestDate;
                tasks.PlannedStart = taskrequest.PlannedStart;
                tasks.Status = "ToDo";
                tasks.User = user;
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PriorityID"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Priority"), "Id", "OptionName");
            ViewData["AssignedTo"] = new SelectList(_context.Users, "Id", "FirstName", tasks.AssignedTo);
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "EpicsName", tasks.EpicsID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", tasks.ProjectID);
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName", tasks.RequestedBy);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName", tasks.CreatedBy);
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmpTasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.EmpTasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }
            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");

            ViewData["PriorityID"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Priority"), "Id", "OptionName",tasks.PriorityID);
            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName", tasks.AssignedTo);
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "EpicsName", tasks.EpicsID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", tasks.ProjectID);
            ViewData["RequestedBy"] = new SelectList(managerList, "Id", "FirstName", tasks.RequestedBy);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName", tasks.CreatedBy);
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TasksRequest taskrequest)
        {
            var tasks = await _context.EmpTasks.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (tasks != null)
                    {
                        tasks.TaskName = taskrequest.TaskName;
                        tasks.PriorityID = taskrequest.PriorityID;
                        tasks.ProjectID = taskrequest.ProjectID;
                        tasks.EpicsID = taskrequest.EpicsID;
                        tasks.DueDate = taskrequest.DueDate;
                        tasks.EstTime = taskrequest.EstTime;
                        tasks.AssignedTo = taskrequest.AssignedTo;
                        tasks.RequestedBy = taskrequest.RequestedBy;
                        tasks.RequestDate = taskrequest.RequestDate;
                        tasks.PlannedStart = taskrequest.PlannedStart;
                        tasks.Status = "ToDo";
                        tasks.ModifiedDate = DateTime.UtcNow;
                    }
                    _context.Update(tasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.Id))
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
            ViewData["AssignedTo"] = new SelectList(_context.Users, "Id", "Id", tasks.AssignedTo);
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "Id", tasks.EpicsID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", tasks.ProjectID);
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "Id", tasks.RequestedBy);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id", tasks.CreatedBy);
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmpTasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.EmpTasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Epics)
                .Include(t => t.Projects)
                .Include(t => t.RequestedUser)
                .Include(t => t.User)
                .Include(t => t.OptionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmpTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ETasks'  is null.");
            }
            var tasks = await _context.EmpTasks.FindAsync(id);
            if (tasks != null)
            {
                _context.EmpTasks.Remove(tasks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasksExists(int id)
        {
          return (_context.EmpTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
