using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class OptionType
    {
        [Key]
        public int  Id { get; set; }

        public string Type { get; set; }

        public int  Value { get; set; }

        public  string OptionName { get; set; }
    }
}
