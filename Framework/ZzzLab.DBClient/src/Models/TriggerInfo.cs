using System.ComponentModel.DataAnnotations;

namespace ZzzLab.Data.Models
{
    public class TriggerInfo
    {
        public string Owner { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        public string TriggerName { get; set; }
    }
}