using System;

namespace AssetProject.ReportModels
{
    public class BrockenModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public string Photo { set; get; }
        public int AssetBrokenId { get; set; }
        public string ItemTL { set; get; }
        public string StoreTL { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryTL { get; set; }
        public string LocationTL { get; set; }
        public int LocationId { get; set; }
        public string DepartmentTL { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateBroken { get; set; }
        public string Notes { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
