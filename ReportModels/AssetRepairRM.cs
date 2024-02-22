using System;

namespace AssetProject.ReportModels
{
    public class AssetRepairRM
    {

        public int AssetRepairId { set; get; }
        public DateTime ScheduleDate { set; get; }
        public DateTime CompletedDate { set; get; }
        public double RepairCost { set; get; }
        public string Notes { set; get; }
        public int TechnicianId { set; get; }
        public string TechnicianName { set; get; }

        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public string photo { set; get; }

    }
}
