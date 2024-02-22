using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public DateTime AssetPurchaseDate { set; get; }
        public Item Item { set; get; }
        public int ItemId { set; get; }
        public string Photo { set; get; }
        public bool DepreciableAsset { set; get; }
        public double? DepreciableCost { set; get; }
        public double? SalvageValue { set; get; }
        public int? AssetLife { set; get; }
        public DateTime? DateAcquired { set; get; }
        public DepreciationMethod DepreciationMethod { set; get; }
        public int? DepreciationMethodId { set; get; }
        public AssetStatus AssetStatus { set; get; }
        public int? AssetStatusId { set; get; }
        public int? VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}
