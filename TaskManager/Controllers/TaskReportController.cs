using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Response;

namespace TaskManager.Controllers
{
    public class TaskReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public TaskReportController(ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(getTaskReport().ToList());
        }

             
        [HttpGet]
        public List<EmployeeTaskReport> getTaskReport ()
        {
            List<EmployeeTaskReport> employees = new List<EmployeeTaskReport>();
            try
            {
                
                using (var database = _context)
                {
                    var connection = _context.Database.GetDbConnection();
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[TaskReport]";
                    var reader = command.ExecuteReader();

                    
                    while (reader.Read())
                    {
                        EmployeeTaskReport employeeTaskReport = new EmployeeTaskReport();
                        employeeTaskReport.ProjectName = reader.IsDBNull("ProjectName") ? null : reader.GetString("ProjectName");
                        employeeTaskReport.EpicName = reader.IsDBNull("EpicsName") ? null : reader.GetString("EpicsName");
                        employeeTaskReport.Priority = reader.IsDBNull("Priority") ? null : reader.GetString("Priority");
                        employeeTaskReport.AssignedTo = reader.IsDBNull("AssignedTo") ? null : reader.GetString("AssignedTo");
                        employeeTaskReport.RequestedBy = reader.IsDBNull("RequestedBy") ? null : reader.GetString("RequestedBy");
                        employeeTaskReport.TaskName = reader.IsDBNull("TaskName") ? null : reader.GetString("TaskName");
                        employeeTaskReport.Status = reader.IsDBNull("Status") ? null : reader.GetString("Status");
                        employeeTaskReport.PlannedStart = reader.GetDateTime("PlannedStart");
                        employeeTaskReport.RequestDate = reader.GetDateTime("RequestDate");
                        employeeTaskReport.ModifiedDate =  reader.GetDateTime("ModifiedDate");
                        employeeTaskReport.DueDate = reader.GetDateTime("DueDate");
                        employees.Add(employeeTaskReport);  
                    }

                    reader.Close();
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
