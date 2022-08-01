using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Request;

namespace TaskManager.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public EmployeeDashboardController(ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EmployeeDashboard
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var applicationDbContext = await _context.EmpTasks
                .Include(e => e.AssignedUser)
                .Include(e => e.Epics)
                .Include(e => e.OptionType)
                .Include(e => e.Projects)
                .Include(e => e.RequestedUser)
                .Include(e => e.User)
                .Include(e => e.StatusType)
                .Where(e => e.AssignedTo == user.Id).ToListAsync();
            return View(applicationDbContext);
        }

        // GET: EmployeeDashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmpTasks == null)
            {
                return NotFound();
            }

            var eTasks = await _context.EmpTasks
                .Include(e => e.AssignedUser)
                .Include(e => e.Epics)
                .Include(e => e.OptionType)
                .Include(e => e.Projects)
                .Include(e => e.RequestedUser)
                .Include(e => e.User)
                .Include(e => e.StatusType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eTasks == null)
            {
                return NotFound();
            }

            return View(eTasks);
        }

        // GET: EmployeeDashboard/Create
        /*public IActionResult Create()
        {
            ViewData["AssignedTo"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "Id");
            ViewData["PriorityID"] = new SelectList(_context.OptionTypes, "Id", "Id");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName");
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }*/

        // POST: EmployeeDashboard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,PriorityID,EstTime,ProjectID,EpicsID,AssignedTo,RequestedBy,RequestDate,PlannedStart,DueDate,Status,Comments,CreatedBy,CreatedDate,ModifiedDate")] ETasks eTasks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eTasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedTo"] = new SelectList(_context.Users, "Id", "Id", eTasks.AssignedTo);
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "Id", eTasks.EpicsID);
            ViewData["PriorityID"] = new SelectList(_context.OptionTypes, "Id", "Id", eTasks.PriorityID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", eTasks.ProjectID);
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "Id", eTasks.RequestedBy);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id", eTasks.CreatedBy);
            return View(eTasks);
        }*/

        // GET: EmployeeDashboard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmpTasks == null)
            {
                return NotFound();
            }

            var eTasks = await _context.EmpTasks.FindAsync(id);
            if (eTasks == null)
            {
                return NotFound();
            }

            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");

            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName", eTasks.AssignedTo);
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "EpicsName", eTasks.EpicsID);
            ViewData["PriorityID"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Priority"), "Id", "OptionName", eTasks.PriorityID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", eTasks.ProjectID);
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName", eTasks.RequestedBy);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id", eTasks.CreatedBy);
            ViewData["Status"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Status"), "Id", "OptionName", eTasks.Status);
            ViewData["SpentTime"] = eTasks.TimeSpent;
            ViewData["Taskname"] = eTasks.TaskName;
            //ViewData["TimeTaken"] = null;
            return View(eTasks);
        }

        // POST: EmployeeDashboard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeTaskRequest eTasks,TimeSpan timetaken)
        {
            var empTasks = await _context.EmpTasks.FindAsync(id);
            var time = timetaken;
            if (id != empTasks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(empTasks != null)
                    {
                        empTasks.TaskName = eTasks.TaskName;
                        empTasks.PriorityID = eTasks.PriorityID;
                        empTasks.ProjectID = eTasks.ProjectID;
                        empTasks.EpicsID = eTasks.EpicsID;
                        empTasks.DueDate = eTasks.DueDate;
                        empTasks.EstTime = eTasks.EstTime;
                        empTasks.AssignedTo = eTasks.AssignedTo;
                        empTasks.RequestedBy = eTasks.RequestedBy;
                        empTasks.RequestDate = eTasks.RequestDate;
                        empTasks.PlannedStart = eTasks.PlannedStart;
                        empTasks.Status = eTasks.Status;
                        empTasks.ModifiedDate = DateTime.UtcNow;
                        empTasks.Comments = eTasks.Comments;
                        empTasks.TimeSpent = empTasks.TimeSpent + timetaken;
                    }
                    _context.Update(empTasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ETasksExists(empTasks.Id))
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
            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");
            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName", eTasks.AssignedTo);
            ViewData["EpicsID"] = new SelectList(_context.Epics, "Id", "EpicsName", eTasks.EpicsID);
            ViewData["PriorityID"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Priority"), "Id", "OptionName", eTasks.PriorityID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", eTasks.ProjectID);
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName", eTasks.RequestedBy);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id", empTasks.CreatedBy);
            ViewData["Status"] = new SelectList(_context.OptionTypes.Where(m => m.Type == "Status"), "Id", "OptionName", eTasks.Status);
            return View(eTasks);
        }

        // GET: EmployeeDashboard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmpTasks == null)
            {
                return NotFound();
            }

            var eTasks = await _context.EmpTasks
                .Include(e => e.AssignedUser)
                .Include(e => e.Epics)
                .Include(e => e.OptionType)
                .Include(e => e.Projects)
                .Include(e => e.RequestedUser)
                .Include(e => e.User)
                .Include(e => e.StatusType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eTasks == null)
            {
                return NotFound();
            }

            return View(eTasks);
        }

        // POST: EmployeeDashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmpTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmpTasks'  is null.");
            }
            var eTasks = await _context.EmpTasks.FindAsync(id);
            if (eTasks != null)
            {
                _context.EmpTasks.Remove(eTasks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ETasksExists(int id)
        {
          return (_context.EmpTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
