using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class EmployeeTask
    {
        public EmployeeTask()
        {
            this.CreatedDate = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Task Name is required.")]
        [Display(Name = "Task")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [Display(Name = "Priority")]
        public int PriorityID { get; set; }

        [Display(Name = "Estimated Time")]
        public TimeSpan EstTime { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        [Display(Name = "Project Name")]
        public virtual int ProjectID { get; set; }
        public virtual Project Projects { get; set; }


        [Required(ErrorMessage = "AssignedTo is required.")]
        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "RequestedBy is required.")]
        [Display(Name = "RequestedBy")]
        public string RequestedBy { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public string? Status { get; set; }
        public string? Comments { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual AppUser User { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
