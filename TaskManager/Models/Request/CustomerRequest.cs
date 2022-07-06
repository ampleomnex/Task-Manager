using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models.Request
{
    public class CustomerRequest
    {
        [Required(ErrorMessage = "Customer Name is required..")]
        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        public string SPOCName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Type { get; set; }
    }
}
