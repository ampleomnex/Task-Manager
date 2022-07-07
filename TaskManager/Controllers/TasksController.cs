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

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TasksController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Get Employees list
        public async Task getEmployees()
        {
            bool employeeRoleExists = await _roleManager.RoleExistsAsync("employee");
            if (employeeRoleExists)
            {
                var resUsers = await _userManager.GetUsersInRoleAsync("employee");
                List<SelectListItem> Employeeid = resUsers
                              .Select(a => new SelectListItem()
                              {
                                  Value = a.Id,
                                  Text = a.FirstName + " " + a.LastName
                              }).ToList();

                ViewData["EmployeeID"] = Employeeid;
            }
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
            ViewBag.PriorityID = Priorityid;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var model = _context.Tasks.Join(_context.Employees,
                t => t.EmployeeID, 
                emp => emp.EmployeeID,
                (t, emp) => new Tasks{
                    Id = t.Id,
                    TaskName= t.TaskName, 
                    PriorityID= t.PriorityID,
                    EstTime= t.EstTime, 
                    EmployeeID= emp.FirstName + " " + emp.LastName,
                    DueDate= t.DueDate 
                });
            return View(model);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
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
            await getEmployees();
            getPriority();
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TasksRequest tasksrequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Tasks tasks = new Tasks();
            if (ModelState.IsValid)
            {
                tasks.TaskName = tasksrequest.TaskName;
                tasks.PriorityID = tasksrequest.PriorityID;
                tasks.EmployeeID = tasksrequest.EmployeeID;
                tasks.DueDate = tasksrequest.DueDate;
                tasks.EstTime = tasksrequest.EstTime;
                tasks.User=user;
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }
            await getEmployees();
            getPriority();
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TasksRequest tasksrequest)
        {
            var tasks = await _context.Tasks.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if(tasks != null)
                    {
                        tasks.TaskName = tasksrequest.TaskName;
                        tasks.PriorityID = tasksrequest.PriorityID;
                        tasks.EmployeeID = tasksrequest.EmployeeID;
                        tasks.DueDate = tasksrequest.DueDate;
                        tasks.EstTime = tasksrequest.EstTime;
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
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
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
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tasks'  is null.");
            }
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks != null)
            {
                _context.Tasks.Remove(tasks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasksExists(int id)
        {
          return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
