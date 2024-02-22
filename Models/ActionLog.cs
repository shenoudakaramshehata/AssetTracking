using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class ActionLog
    {
        [Key]
        public int ActionLogId { get; set; }
        public string ActionLogTitle { get; set; }
    }
}
