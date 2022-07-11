using System;
using System.Collections;
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
            var applicationDbContext = _context.EmployeeDetails.Include(e => e.Departments)
               .Include(e => e.ReportingUser)
               .Include(e => e.Roles)
               .Include(e => e.Functions)
               .Include(e => e.Teams)
               .Include(e => e.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmployeesDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeDetails == null)
            {
                return NotFound();
            }

            var employeesDetails = await _context.EmployeeDetails
                .Include(e => e.Departments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeesDetails == null)
            {
                return NotFound();
            }

            return View(employeesDetails);
        }

        // GET: EmployeesDetails/Create
        public async Task<IActionResult> CreateAsync()
        {
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");
            IEnumerable rolelist = _roleManager.Roles;

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName");
            ViewData["FunctionID"] = new SelectList(_context.Functions, "Id", "FunctionName");
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "TeamName");
            ViewData["Reportingto"] = new SelectList(managerList, "Id", "FirstName");
            ViewData["RoleName"] = new SelectList(rolelist, "Id", "Name");
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName");

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


                    EmployeeDetails employees = new EmployeeDetails();
                    employees.User = await _userManager.GetUserAsync(HttpContext.User); 
                    employees.EmployeeID = AspNetUsers.Id.ToString();
                    employees.Reportingto = employeesrequest.Reportingto;
                    employees.Email = employeesrequest.Email;
                    employees.FirstName = employeesrequest.FirstName;
                    employees.LastName = employeesrequest.LastName;
                    employees.DepartmentID = employeesrequest.DepartmentID;
                    employees.PhoneNumber = employeesrequest.PhoneNumber;
                    employees.RoleName = employeesrequest.RoleName;
                    employees.FunctionID = employeesrequest.FunctionID;
                    employees.TeamID = employeesrequest.TeamID;
                    _context.Add(employees);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }               
               
            }

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName");
            ViewData["FunctionID"] = new SelectList(_context.Functions, "Id", "FunctionName");
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "TeamName");
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName");
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
            if (id == null || _context.EmployeeDetails == null)
            {
                return NotFound();
            }

            var employeesDetails = await _context.EmployeeDetails.FindAsync(id);
            if (employeesDetails == null)
            {
                return NotFound();
            }
            IEnumerable managerList = await _userManager.GetUsersInRoleAsync("manager");
            IEnumerable rolelist = _roleManager.Roles;

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "DepartmentName", employeesDetails.DepartmentID);
            ViewData["FunctionID"] = new SelectList(_context.Functions, "Id", "FunctionName", employeesDetails.FunctionID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "TeamName",employeesDetails.TeamID);
            ViewData["Reportingto"] = new SelectList(managerList, "Id", "FirstName", employeesDetails.Reportingto);
            ViewData["RoleName"] = new SelectList(rolelist, "Id", "Name", employeesDetails.RoleName);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName", employeesDetails.CreatedBy);
            return View(employeesDetails);
        }

        // POST: EmployeesDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeesDetailsRequest employeesrequest)
        {
            var employeesDetails = await _context.EmployeeDetails.FindAsync(id);
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeesDetails != null)
                    {                        
                        employeesDetails.Reportingto = employeesrequest.Reportingto;
                        employeesDetails.Email = employeesrequest.Email;
                        employeesDetails.FirstName = employeesrequest.FirstName;
                        employeesDetails.LastName = employeesrequest.LastName;
                        employeesDetails.DepartmentID = employeesrequest.DepartmentID;
                        employeesDetails.PhoneNumber = employeesrequest.PhoneNumber;
                        employeesDetails.RoleName = employeesrequest.RoleName;
                        employeesDetails.FunctionID = employeesrequest.FunctionID;
                        employeesDetails.TeamID = employeesrequest.TeamID;
                        employeesDetails.ModifiedDate = DateTime.UtcNow;
                    }
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
            ViewData["FunctionID"] = new SelectList(_context.Functions, "Id", "FunctionName", employeesDetails.FunctionID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "TeamName", employeesDetails.TeamID);
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "FirstName", employeesDetails.CreatedBy);
            return View(employeesDetails);
        }

        // GET: EmployeesDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeDetails == null)
            {
                return NotFound();
            }

            var employeesDetails = await _context.EmployeeDetails
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
            if (_context.EmployeeDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employeesDetails = await _context.EmployeeDetails.FindAsync(id);
            if (employeesDetails != null)
            {
                _context.EmployeeDetails.Remove(employeesDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesDetailsExists(int id)
        {
          return (_context.EmployeeDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
