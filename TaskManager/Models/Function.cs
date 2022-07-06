using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Function
    {
        public Function()
        {
            this.CreatedDate = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Function is required..")]
        [Display(Name = "Function Name")]
        public string FunctionName { get; set; }

        [Required(ErrorMessage = "Department is required..")]
        [Display(Name = "Department Name")]
        public virtual int DepartmentID { get; set; }
        public virtual Department Departments { get; set; }

        public virtual AppUser User { get; set; }

        public DateTime CreatedDate { get; set; }


    }
}
