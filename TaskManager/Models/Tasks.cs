using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Tasks
    {
        public Tasks()
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

        [Required(ErrorMessage = "Employee is required.")]
        [Display(Name = "Employee")]
        public string EmployeeID { get; set; }

        public virtual AppUser User { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
