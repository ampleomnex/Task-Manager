using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class ETasks
    {
        public ETasks()
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
        public virtual int PriorityID { get; set; }
        [ForeignKey("PriorityID")]
        public virtual OptionType OptionType { get; set; }

        [Display(Name = "Estimated Time")]
        public TimeSpan EstTime { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        [Display(Name = "Project Name")]
        public virtual int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Projects Projects { get; set; }

        [Required(ErrorMessage = "Epic is required.")]
        [Display(Name = "Epic")]
        public virtual int EpicsID { get; set; }
        public virtual Epics Epics { get; set; }


        [Required(ErrorMessage = "AssignedTo is required.")]
        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }
        [ForeignKey("AssignedTo")]
        public virtual AppUser AssignedUser { get; set; }

        [Required(ErrorMessage = "RequestedBy is required.")]
        [Display(Name = "RequestedBy")]
        public string RequestedBy { get; set; }
        [ForeignKey("RequestedBy")]
        public virtual AppUser RequestedUser { get; set; }

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Planned Start")]
        public DateTime PlannedStart { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public virtual int Status { get; set; }
        [ForeignKey("Status")]

        public virtual OptionType StatusType { get; set; }
        public string? Comments { get; set; }

        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual AppUser User { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
