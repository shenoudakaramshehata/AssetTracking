using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class SellAssetModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public DateTime SaleDate { get; set; }
        public string SoldTo { get; set; }
        public double SaleAmount { get; set; }
        public string SellNotes { get; set; }
        public string photo { get; set; }
    }
}
