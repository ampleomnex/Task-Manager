using System;
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
    public class FunctionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FunctionsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Functions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Functions.Include(f => f.Departments);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Functions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Functions == null)
            {
                return NotFound();
            }

            var function = await _context.Functions
                .Include(f => f.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // GET: Functions/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName");
            return View();
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FunctionRequest functionrequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Function function = new Function();
            if (ModelState.IsValid)
            {
                function.FunctionName = functionrequest.FunctionName;
                function.DepartmentID = functionrequest.DepartmentID;
                function.User = user;
                function.CreatedDate = DateTime.Now;               
                _context.Add(function);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", function.DepartmentID);
            return View();
        }

        // GET: Functions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Functions == null)
            {
                return NotFound();
            }

            var function = await _context.Functions.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", function.DepartmentID);
            return View(function);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FunctionRequest functionrequest)
        {
            var function = await _context.Functions.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if(function != null)
                    {
                        function.FunctionName = functionrequest.FunctionName;
                        function.DepartmentID = functionrequest.DepartmentID;
                    }
                    _context.Update(function);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionExists(function.Id))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", function.DepartmentID);
            return View(function);
        }

        // GET: Functions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Functions == null)
            {
                return NotFound();
            }

            var function = await _context.Functions
                .Include(f => f.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // POST: Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Functions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Functions'  is null.");
            }
            var function = await _context.Functions.FindAsync(id);
            if (function != null)
            {
                _context.Functions.Remove(function);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionExists(int id)
        {
          return (_context.Functions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
