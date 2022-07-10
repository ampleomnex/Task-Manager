using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Request;
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
        public async Task<IActionResult> Index()
        {
            await getEmployees();
            await getPriority();
            return View(getTaskReport().ToList());
        }

        public async Task getEmployees()
        {
            IEnumerable employeeList = await _userManager.GetUsersInRoleAsync("employee");            
            ViewData["AssignedTo"] = new SelectList(employeeList, "Id", "FirstName");
        }

        public async Task getPriority()
        {
            ViewData["Priority"] = new SelectList(_context.OptionTypes, "Id", "OptionName");

        }



        [HttpGet]
        public async Task<IActionResult> EmployeeReport(string employeeId,string Param1)
        {
            View();
            return View(getTaskReportByEmployee(employeeId).ToList());
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
                    //command.Parameters.Add(new SqlParameter("@StartDate", "10-7-2022"));
                    //command.Parameters.Add(new SqlParameter("@EndDate", "17-07-2022"));
                    //command.Parameters.Add(new SqlParameter("@Priority", "high"));
                    //command.Parameters.Add(new SqlParameter("@AssignedTo", "f0d40e20-4659-4f52-8cf9-ca90df4bda40"));
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
                    reader.NextResult();

                    reader.Close();
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public List<EmployeeTaskReport> getTaskReportByEmployee(String empID)
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
                    command.CommandText = "[dbo].[IndividualEmployeeTaskReport]";
                    //[TaskFilter]
                   // command.CommandText = "[dbo].[TaskFilter]";
                    //command.Parameters.Add(new SqlParameter("@StartDate", "10-7-2022"));
                    //command.Parameters.Add(new SqlParameter("@EndDate", "17-07-2022"));
                    //command.Parameters.Add(new SqlParameter("@Priority", "high"));
                    command.Parameters.Add(new SqlParameter("@AssignedTo", empID));
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
                        employeeTaskReport.ModifiedDate = reader.GetDateTime("ModifiedDate");
                        employeeTaskReport.DueDate = reader.GetDateTime("DueDate");
                        employees.Add(employeeTaskReport);
                    }
                    reader.NextResult();

                    reader.Close();
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public List<EmployeeTaskReport> getTaskCompletedOnTimeByEmployee(String empID, int Priority)
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
                    command.CommandText = "[dbo].[TaskCompletedOnTime]";
                    //command.Parameters.Add(new SqlParameter("@StartDate", "10-7-2022"));
                    //command.Parameters.Add(new SqlParameter("@EndDate", "17-07-2022"));
                    //command.Parameters.Add(new SqlParameter("@Priority", "high"));
                    command.Parameters.Add(new SqlParameter("@AssignedTo", empID));
                    command.Parameters.Add(new SqlParameter("@PriorityID", Priority));
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
                        employeeTaskReport.ModifiedDate = reader.GetDateTime("ModifiedDate");
                        employeeTaskReport.DueDate = reader.GetDateTime("DueDate");
                        employees.Add(employeeTaskReport);
                    }
                    reader.NextResult();

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
