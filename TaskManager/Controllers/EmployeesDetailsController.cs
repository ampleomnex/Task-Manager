using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Request;

namespace TaskManager.Controllers
{
    public class EmployeesDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<AppUser> _userStore;
        //private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public EmployeesDetailsController(ApplicationDbContext context, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IUserStore<AppUser> userStore,
            IEmailSender emailSender, IMapper mapper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            //_emailStore = emailStore;
            _mapper = mapper;
        }

        // GET: EmployeesDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.Departments);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmployeesDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employeesDetails = await _context.Employees
                .Include(e => e.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeesDetails == null)
            {
                return NotFound();
            }

            return View(employeesDetails);
        }

        // GET: EmployeesDetails/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName");
            return View();
        }

        // POST: EmployeesDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeesDetailsRequest employeesrequest)
        {
            if (ModelState.IsValid)
            {
                User user = new User(employeesrequest.FirstName,employeesrequest.LastName,employeesrequest.Email,employeesrequest.Email, Guid.NewGuid());
                
                                
                bool employeeRoleExists = await _roleManager.RoleExistsAsync("employee");
                if (!employeeRoleExists)
                {
                    IdentityRole role = new IdentityRole()
                    {
                        Name = "employee",
                        NormalizedName = "EMPLOYEE"
                    };
                    var AspNetRoles = _mapper.Map<IdentityRole>(role);
                    await _roleManager.CreateAsync(AspNetRoles);
                }

               
               // await _userStore.SetUserNameAsync(user, employeesrequest.Email, CancellationToken.None);
               // await _emailStore.SetEmailAsync((AppUser)user, employeesDetailsRequest.Email, CancellationToken.None);

                var password = "Test@123";
                var AspNetUsers = _mapper.Map<AppUser>(user);
                var result = await _userManager.CreateAsync(AspNetUsers, password);
                if (!await _userManager.IsInRoleAsync(AspNetUsers, "employee"))
                {
                    var userResult = await _userManager.AddToRoleAsync(AspNetUsers, "employee");
                }
                if(result.Succeeded)
                {

                    var userId = await _userManager.GetUserIdAsync(AspNetUsers);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(AspNetUsers);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    var emailConfirmresult = await _userManager.ConfirmEmailAsync(AspNetUsers, code);


                    EmployeesDetails employees = new EmployeesDetails();
                    employees.User = (AppUser)AspNetUsers;
                    employees.EmployeeID = AspNetUsers.Id.ToString();
                    employees.Reportingto = employeesrequest.Reportingto;
                    employees.Email = employeesrequest.Email;
                    employees.FirstName = employeesrequest.FirstName;
                    employees.LastName = employeesrequest.LastName;
                    employees.DepartmentID = employeesrequest.DepartmentID;

                    _context.Add(employees);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }               
               
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", employeesrequest.DepartmentID);
            return View();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        // GET: EmployeesDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employeesDetails = await _context.Employees.FindAsync(id);
            if (employeesDetails == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", employeesDetails.DepartmentID);
            return View(employeesDetails);
        }

        // POST: EmployeesDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,DepartmentID,Reportingto,EmployeeID,CreatedDate")] EmployeesDetails employeesDetails)
        {
            if (id != employeesDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeesDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesDetailsExists(employeesDetails.Id))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", employeesDetails.DepartmentID);
            return View(employeesDetails);
        }

        // GET: EmployeesDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employeesDetails = await _context.Employees
                .Include(e => e.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeesDetails == null)
            {
                return NotFound();
            }

            return View(employeesDetails);
        }

        // POST: EmployeesDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employeesDetails = await _context.Employees.FindAsync(id);
            if (employeesDetails != null)
            {
                _context.Employees.Remove(employeesDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesDetailsExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
