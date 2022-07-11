using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class EmployeeDetails
    {
        public EmployeeDetails()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Department is required..")]
        [Display(Name = "Department")]
        public virtual int DepartmentID { get; set; }
        public virtual Department Departments { get; set; }

        [Display(Name = "Reporting To")]
        public string Reportingto { get; set; }
        [ForeignKey("Reportingto")]
        public virtual AppUser ReportingUser { get; set; }

        [Display(Name = "Role")]
        public virtual string RoleName { get; set; }
        [ForeignKey("RoleName")]
        public virtual IdentityRole Roles { get; set; }

        [Required(ErrorMessage = "Function Name is required.")]
        [Display(Name = "Function")]
        public virtual int FunctionID { get; set; }
        public virtual Function Functions { get; set; }

        [Required(ErrorMessage = "Team Name is required.")]
        [Display(Name = "Team")]
        public virtual int TeamID { get; set; }
        public virtual Team Teams { get; set; }

        public string EmployeeID { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual AppUser User { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
