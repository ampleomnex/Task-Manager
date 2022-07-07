using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class EpicsRequest
    {
        [Required(ErrorMessage = "Epics Name is required..")]
        [Display(Name = "Epics")]
        public string EpicsName { get; set; }

        public int ProjectID { get; set; }
                
    }
}
