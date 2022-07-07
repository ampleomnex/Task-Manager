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
    public class EpicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EpicsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Epics.Include(e => e.Projects).Include(e => e.User);
            return View(await applicationDbContext.ToListAsync());
            //return _context.Epics != null ? 
            //View(await _context.Epics.ToListAsync()) :
            //Problem("Entity set 'ApplicationDbContext.Epics'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Epics == null)
            {
                return NotFound();
            }

            var epics = await _context.Epics
                .Include(e => e.Projects)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (epics == null)
            {
                return NotFound();
            }

            return View(epics);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( EpicsRequest epicsrequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Epics epics = new Epics();

            if (ModelState.IsValid)
            {
                epics.ProjectID = epicsrequest.ProjectID;
                epics.EpicsName = epicsrequest.EpicsName;
                epics.User = user;
                               
                _context.Add(epics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(epics);
        }

        // GET: Epics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Epics == null)
            {
                return NotFound();
            }

            var epics = await _context.Epics.FindAsync(id);
            if (epics == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectName", epics.ProjectID);

            return View(epics);
        }

        // POST: Epics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EpicsRequest epicsrequest)
        {
            var epics = await _context.Epics.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if(epics != null)
                    {
                        epics.EpicsName = epicsrequest.EpicsName;
                        epics.ProjectID = epicsrequest.ProjectID;

                    }
                    _context.Update(epics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpicsExists(epics.Id))
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
            return View(epics);
        }

        // GET: Epis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Epics == null)
            {
                return NotFound();
            }

            var epics = await _context.Epics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (epics == null)
            {
                return NotFound();
            }

            return View(epics);
        }

        // POST: Epics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Epics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Epics'  is null.");
            }
            var epics = await _context.Epics.FindAsync(id);
            if (epics != null)
            {
                _context.Epics.Remove(epics);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpicsExists(int id)
        {
          return (_context.Epics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
