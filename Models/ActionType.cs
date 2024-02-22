using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class ActionType
    {
        [Key]
        public int ActionTypeId { set; get; }
        public string ActionTypeTitle { set; get; }
    }
}
