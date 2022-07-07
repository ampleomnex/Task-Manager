using System;
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
using System.Collections;

namespace TaskManager.Controllers
{
    public class EmployeeTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeTasksController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: EmployeeTasks
        public async Task<IActionResult> Index()
        {
            /*var model = (
                    from t in _context.EmployeeTasks
                    join e in _context.Employees on t.AssignedTo equals e.EmployeeID
                    join emp in _context.Employees on t.RequestedBy equals emp.EmployeeID
                    select new EmployeeTask 
                    { 
                        Id = t.Id,
                        TaskName = t.TaskName,
                        PriorityID = t.PriorityID,
                        ProjectID = t.ProjectID,
                        AssignedTo = e.FirstName,
                        RequestedBy = emp.FirstName,
                        EstTime = t.EstTime,
                        DueDate = t.DueDate                        
                    }
                    ).ToList();
            return View(model);*/


            /*var model = _context.EmployeeTasks.Join(_context.Employees,
                t => t.AssignedTo, 
                emp => emp.EmployeeID,
                (t, emp) => new EmployeeTask
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    PriorityID = t.PriorityID,
                    EstTime = t.EstTime,
                    AssignedTo = emp.FirstName + " " + emp.LastName,
                    RequestedBy = emp.FirstName + " " + emp.LastName,
                    DueDate = t.DueDate
                });
            return View(model);*/

            var applicationDbContext = _context.EmployeeTasks.Include(e => e.Projects).Include(e => e.User);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task getPriority()
        {
            List<SelectListItem> Priorityid = new List<SelectListItem>() {
                                    new SelectListItem {
                                        Text = "Immediate", Value = "1"
                                    },new SelectListItem {
                                        Text = "High", Value = "2"
                                    },
                                    new SelectListItem{
                                        Text = "Medium", Value = "3"
                                    },
                                    new SelectListItem{
                                        Text = "Low", Value = "4"
                                    }
                                };
            IEnumerable Priorities = Priorityid;
            ViewBag.PriorityID = new SelectList(Priorities, "Value", "Text"); ;
        }

        // GET: EmployeeTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeTasks == null)
            {
                return NotFound();
            }


            var employeeTask = await _context.EmployeeTasks
                .Include(e => e.Projects)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employeeTask == null)
            {
                return NotFound();
            }

            return View(employeeTask);
        }

        // GET: EmployeeTasks/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager"); 

            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName");
            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName");
            ViewData["RequestedBy"] = new SelectList(managerList, "Id", "FirstName");
            getPriority();
            return View();
        }

        // POST: EmployeeTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TasksRequest taskrequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            EmployeeTask employeeTask = new EmployeeTask();
            if (ModelState.IsValid)
            {
                employeeTask.TaskName = taskrequest.TaskName;
                employeeTask.PriorityID = taskrequest.PriorityID;
                employeeTask.ProjectID = taskrequest.ProjectID;
                employeeTask.DueDate = taskrequest.DueDate;
                employeeTask.EstTime = taskrequest.EstTime;
                employeeTask.AssignedTo = taskrequest.AssignedTo;
                employeeTask.RequestedBy = taskrequest.RequestedBy;
                employeeTask.User = user;
                _context.Add(employeeTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", taskrequest.ProjectID);
            ViewData["AssignedTo"] = new SelectList(_context.Users, "Id", "UserName", taskrequest.AssignedTo);
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "UserName", taskrequest.RequestedBy);
            return View(employeeTask);
        }

        // GET: EmployeeTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeTasks == null)
            {
                return NotFound();
            }

            var employeeTask = await _context.EmployeeTasks.FindAsync(id);
            if (employeeTask == null)
            {
                return NotFound();
            }
            getPriority();
            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");

            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", employeeTask.ProjectID);
            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName", employeeTask.AssignedTo);
            ViewData["RequestedBy"] = new SelectList(managerList, "Id", "FirstName", employeeTask.RequestedBy);
            return View(employeeTask);
        }

        // POST: EmployeeTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TasksRequest taskrequest)
        {
            var employeeTask = await _context.EmployeeTasks.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeTask != null)
                    {
                        employeeTask.TaskName = taskrequest.TaskName;
                        employeeTask.PriorityID = taskrequest.PriorityID;
                        employeeTask.ProjectID = taskrequest.ProjectID;
                        employeeTask.DueDate = taskrequest.DueDate;
                        employeeTask.EstTime = taskrequest.EstTime;
                        employeeTask.AssignedTo = taskrequest.AssignedTo;
                        employeeTask.RequestedBy = taskrequest.RequestedBy;
                    }
                    _context.Update(employeeTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeTaskExists(employeeTask.Id))
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
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", employeeTask.ProjectID);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id", employeeTask.CreatedBy);
            return View(employeeTask);
        }

        // GET: EmployeeTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeTasks == null)
            {
                return NotFound();
            }

            var employeeTask = await _context.EmployeeTasks
                .Include(e => e.Projects)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeTask == null)
            {
                return NotFound();
            }

            return View(employeeTask);
        }

        // POST: EmployeeTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeTasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployeeTasks'  is null.");
            }
            var employeeTask = await _context.EmployeeTasks.FindAsync(id);
            if (employeeTask != null)
            {
                _context.EmployeeTasks.Remove(employeeTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeTaskExists(int id)
        {
          return (_context.EmployeeTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
