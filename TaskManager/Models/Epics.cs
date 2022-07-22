using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class Epics
    {
        public Epics()
        {
            this.CreatedDate = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }
        public string EpicsName { get; set; }
        [Required(ErrorMessage = "Project Name is required.")]
        [Display(Name = "Project Name")]

        public virtual int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Projects Projects { get; set; }
        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual AppUser User { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
