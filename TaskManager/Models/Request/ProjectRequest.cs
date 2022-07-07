using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class ProjectRequest
    {
        public string ProjectName { get; set; }

        public String spoc { get; set; }

        public int CustomerID { get; set; }

    }
}
