using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Team
    {
        public Team()
        {
            this.CreatedDate = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Team Name is required..")]
        [Display(Name = "Team")]
        public string TeamName { get; set; }

        [Required(ErrorMessage = "Department is required..")]
        [Display(Name = "Department Name")]
        public virtual int DepartmentID { get; set; }

        public virtual Department Departments { get; set; }

        public virtual AppUser User { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
