using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class WarrantyModel
    {
        public int WarrantyId { get; set; }
        public int Length { get; set; }
       
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public int AssetId { get; set; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public string photo { set; get; }
    }
}
