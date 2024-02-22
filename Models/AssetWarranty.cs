using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetWarranty
    {
        [Key]
        public int WarrantyId { get; set; }
        [Required]
        public int Length { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
    }
}
