using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class FunctionRequest
    {
        [Required(ErrorMessage = "Function Name is required..")]
        [Display(Name = "Function")]
        public string FunctionName { get; set; }

        public virtual int DepartmentID { get; set; }

    }
}
