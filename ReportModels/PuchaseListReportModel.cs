using AssetProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class PuchaseListReportModel
    {
        //public int AssetId { set; get; }
        //public string AssetDescription { set; get; }
        //public string AssetTagId { set; get; }
        //public double AssetCost { set; get; }
        //public string AssetSerialNo { set; get; }
        public int PurchaseId { get; set; }
        //public DateTime AssetPurchaseDate { set; get; }
        public string ItemTL { set; get; }
        //public string Photo { set; get; }
        public string CategoryTL { set; get; }
        public string BrandTL { set; get; }
        public string PurchaseSerial { get; set; }
        public DateTime? Purchasedate { get; set; }
        public double? Total { get; set; }
        public double? Discount { get; set; }
        public double? Net { get; set; }

        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TotalPurchaseAsset { get; set; }
        public double? DiscountPurchaseAsset { get; set; }
        public double? NetPurchaseAsset { get; set; }

        //public bool DepreciableAsset { set; get; }
        //public double? DepreciableCost { set; get; }
        //public double? SalvageValue { set; get; }
        //public int? AssetLife { set; get; }
    }
}
