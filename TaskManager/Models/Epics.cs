using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Epics
    {

        [Key]
        public int Id { get; set; }
        public string EpicsName { get; set; }
        public string ProjectName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
