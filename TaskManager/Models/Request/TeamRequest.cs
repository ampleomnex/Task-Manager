using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class TeamRequest
    {
        [Required(ErrorMessage = "Team Name is required..")]
        [Display(Name = "Team")]
        public string TeamName { get; set; }

        public virtual int DepartmentID { get; set; }

    }
}
