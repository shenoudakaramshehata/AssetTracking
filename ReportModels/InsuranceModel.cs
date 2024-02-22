using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class InsuranceModel
    {
        public int AssetId { set; get; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public string ItemTL { set; get; }
        public string Photo { set; get; }
        public int? InsuranceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InsuranceCompany { get; set; }
        public string ContactPerson { get; set; }
        public string PolicyNo { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Deductible { get; set; }
        public decimal Permium { get; set; }
        public bool IsActive { get; set; }
    }
}
