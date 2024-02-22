using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetProject.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }
        [MaxLength(50)]
        public string PurchaseSerial { get; set; }
        public DateTime? Purchasedate { get; set; }
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }
        public int? VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public double? Total { get; set; }
        public double? Discount { get; set; }
        public double? Net { get; set; }
        public string Remarks { get; set; }
        
        public virtual ICollection<PurchaseAsset> PurchaseAssets { get; set; }


    }
}
