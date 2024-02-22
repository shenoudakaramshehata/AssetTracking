using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.ReportModels
{
    public class AssetReportsModel
    {
        public int AssetID { get; set; }
        public string AssetDescription { set; get; }
        public string AssetTagId { set; get; }
        public double AssetCost { set; get; }
        public string AssetSerialNo { set; get; }
        public DateTime? AssetPurchaseDate { set; get; }
        public string ItemTL { set; get; }
        public string Photo { set; get; }
        public bool DepreciableAsset { set; get; }
        public double? DepreciableCost { set; get; }
        public double? SalvageValue { set; get; }
        public int? AssetLife { set; get; }
        public DateTime? DateAcquired { set; get; }
        public string DepreciationMethodTL { set; get; }
        public string AssetStatusTL { set; get; }
        public string VendorTL { get; set; }
        public string StoreTL { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryTL { get; set; }
        public string DepartmentTL { get; set; }
        public string LocationTL { get; set; }
        public int? LocationId { get; set; }
        public int? DepartmentId { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public DateTime? LogActionDate { get; set; }
        public int? StoreId { get; set; }
        public int? ItemId { get; set; }
        public int? VendorId { get; set; }
        public DateTime? TransactionDate { set; get; }
        public int WarrantyId { get; set; }
        public int WarrantyLenght { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public DateTime? DueDate { set; get; }

    }
}
