using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class EmployeesDetails
    {
        public EmployeesDetails()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required..")]
        [Display(Name = "Department Name")]
        public virtual int DepartmentID { get; set; }

        public virtual Department Departments { get; set; }

        public string Reportingto { get; set; }

        public string EmployeeID { get; set; }
        //public virtual int FunctionID { get; set; }
        //public virtual Function Functions { get; set; }

        //public virtual int TeamID { get; set; }
        //public virtual Team Teams { get; set; }
        public virtual AppUser User { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
