using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Request
{
    public class EmployeesDetailsRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required..")]
        [Display(Name = "Department Name")]
        public virtual int DepartmentID { get; set; }        

        public string Reportingto { get; set; }

       // public virtual int FunctionID { get; set; }
       // public virtual Function Functions { get; set; }

       // public virtual int TeamID { get; set; }
       // public virtual Team Teams { get; set; }


    }
}
