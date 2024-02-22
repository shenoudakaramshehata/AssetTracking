using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ViewModel
{
    public class AssetVm2
    {
        public int AssetId { get; set; }
        public string AssetDescription { get; set; }
        public string AssetTagId { get; set; }
        public double AssetCost { get; set; }
        public string AssetSerialNo { get; set; }
        public DateTime AssetPurchaseDate { get; set; }
        public int? ItemId { get; set; }
        public string Photo { get; set; }
        public string PurchaseNo { get; set; }
        public bool DepreciableAsset { get; set; }
        public double DepreciableCost { get; set; }

        public double? SalvageValue { get; set; }
        public int? AssetLife { get; set; }
        public DateTime? DateAcquired { get; set; }
        public int? DepreciationMethodId { get; set; }
        public int? AssetStatusId { get; set; }
        public int? VendorId { get; set; }
        public int? TenantId { get; set; }
        public int? StoreId { get; set; }
    }
}
