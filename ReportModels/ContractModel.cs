using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class ContractModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public string ItemTL { set; get; }
        public string Photo { set; get; }
        public string ContractTL { set; get; }
        public string ContractNo { set; get; }
        public double Cost { set; get; }
        public DateTime ContractStartDate { set; get; }
        public DateTime ContractEndDate { set; get; }
        public int? ContractId { get; set; }

    }
}
