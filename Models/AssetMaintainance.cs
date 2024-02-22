using System;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class AssetMaintainance
    {
        public int AssetMaintainanceId { set; get; }
        [Required]
        public string AssetMaintainanceTitle { set; get; }
        public string AssetMaintainanceDetails { set; get; }
        public DateTime? AssetMaintainanceDueDate { set; get; }
        public MaintainanceStatus MaintainanceStatus { set; get; }
        public int? MaintainanceStatusId { set; get; }
        public DateTime? AssetMaintainanceDateCompleted { set; get; }
        public double AssetMaintainanceRepairesCost { set; get; }
        public bool AssetMaintainanceRepeating { set; get; }
        public AssetMaintainanceFrequency AssetMaintainanceFrequency { set; get; }
        public int? AssetMaintainanceFrequencyId { set; get; }
        public Technician Technician { set; get; }
        public int? TechnicianId { set; get; }
        public virtual Asset Asset { set; get; }
        public int AssetId { set; get; }
        public int? WeeklyPeriod { set; get; }
        public WeekDay WeekDay { set; get; }
        public int? WeekDayId { set; get; }
        public int? MonthlyPeriod { set; get; }
        public int? MonthlyDay { set; get; }
        public Month Month { set; get; }
        public int? MonthId { set; get; }
        public int? YearlyDay { set; get; }
        public DateTime ScheduleDate { get; set; }
    }
}
