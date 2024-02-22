using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AssetProject.Models
{
    public class AssetMovement
    {
        [Key]
        public int AssetMovementId { set; get; }
        public DateTime? TransactionDate { set; get; }
        [ForeignKey("Employee")]
        public int? EmpolyeeID { set; get; }
        public Employee Employee { set; get; }
        public int? LocationId{ set; get; }
        public Location Location { set; get; }
        public int? DepartmentId { set; get; }
        public Department Department { set; get; }
        public int? StoreId { set; get; }
        public Store Store { set; get; }
        public int? ActionTypeId { set; get; }
        public ActionType ActionType { set; get; }
        [JsonIgnore]
        public AssetMovementDirection AssetMovementDirection { set; get; }
        public int? AssetMovementDirectionId { set; get; }
        public string Remarks { set; get; }
        [JsonIgnore]
        public ICollection<AssetMovementDetails> AssetMovementDetails { get; set; }
        public DateTime? DueDate { set; get; }

    }
}
