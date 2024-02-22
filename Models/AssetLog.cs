using System;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class AssetLog
    {
        [Key]
        public int AssetLogId { get; set; }
        public DateTime ActionDate { get; set; }
        public string Remark { get; set; }
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
        public int ActionLogId { get; set; }
        public virtual ActionLog ActionLog { get; set; }
    }
}
