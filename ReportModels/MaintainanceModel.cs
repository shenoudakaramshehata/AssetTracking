using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class MaintainanceModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public string AssetMaintainanceTitle { set; get; }
        public DateTime? AssetMaintainanceDueDate { set; get; }
        public string MaintainanceStatusTL { set; get; }
        public DateTime? AssetMaintainanceDateCompleted { set; get; }
        public double AssetMaintainanceRepairesCost { set; get; }
        public string AssetMaintainanceFrequencyTl { set; get; }
        public string TechnicianName { set; get; }
        public string WeekDayTl { set; get; }
        public string MonthTl { set; get; }
        public int? TechnicianId { set; get; }
        public DateTime? ScheduleDate { set; get; }
        public int? MaintStatusId { set; get; }
        public string photo { set; get; }


    }
}
