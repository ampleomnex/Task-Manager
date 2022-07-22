using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class Projects
    {
        public Projects()
        {
            this.CreatedDate = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Name is required..")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "SPOC is required..")]
        public String spoc { get; set; }
        [ForeignKey("spoc")]
        public virtual AppUser user { get; set; }


        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customers { get; set; }

        public virtual string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual AppUser CreatedUser { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
