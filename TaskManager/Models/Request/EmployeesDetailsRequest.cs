using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Request
{
    public class EmployeesDetailsRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public virtual int DepartmentID { get; set; }        

        public string Reportingto { get; set; }

        public string RoleName { get; set; }

        public virtual int FunctionID { get; set; }

        public virtual int TeamID { get; set; }

        // public virtual int FunctionID { get; set; }
        // public virtual Function Functions { get; set; }

        // public virtual int TeamID { get; set; }
        // public virtual Team Teams { get; set; }


    }
}
