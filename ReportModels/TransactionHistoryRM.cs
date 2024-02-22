using System;

namespace AssetProject.ReportModels
{
    public class TransactionHistoryRM
    {
        public int AssetLogId { get; set; }
        public DateTime ActionDate { get; set; }
        public string Remark { get; set; }
        public int AssetId { get; set; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public int ActionLogId { get; set; }
        public string ActionLogTitle { get; set; }
        public string photo { get; set; }
    }
}
