using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class TasksRequest
    {
        public string TaskName { get; set; }

        public int PriorityID { get; set; }
        
        public TimeSpan EstTime { get; set; }

        public virtual int ProjectID { get; set; }

        public string AssignedTo { get; set; }

        public string RequestedBy { get; set; }

        public DateTime DueDate { get; set; }

        //public string Status { get; set; }
        //public string Comments { get; set; }
    }
}
