using System;
using System.Collections.Generic;

namespace AssetProject.Models
{
    public class AssetRepair
    {
        public int AssetRepairId { set; get; }
        public DateTime ScheduleDate { set; get; }
        public DateTime CompletedDate { set; get; }
        public double RepairCost { set; get; }
        public string Notes { set; get; }
        public int TechnicianId { set; get; }
        public Technician Technician { set; get; }

        public ICollection<AssetRepairDetails> AssetRepairDetails { get; set; }
    }
}
