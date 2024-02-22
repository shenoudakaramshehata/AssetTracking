using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class LeasingModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public int AssetLeasingId { get; set; }
        public DateTime LeasingStartDate { get; set; }
        public DateTime LeasingEndDate { get; set; }
        public string CustomerTL { get; set; }
        public int CustomerId { get; set; }
        public Double LeasingCost { get; set; }
        public DateTime TransactionDate { get; set; }
        public string photo { get; set; }

    }
}
