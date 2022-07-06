using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class DepartmentRequest
    {
        [Required(ErrorMessage = "Department Name is required..")]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
        
        //public virtual AppUser User { get; set; }

    }
}
