using System;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Contract
    {
        public int ContractId { set; get; }
        [Required]
        public string Title { set; get; }
        public string Description { set; get; }
        [Required]
        public string ContractNo { set; get; }

        public double Cost { set; get; }
 
        public DateTime StartDate { set; get; }
     
        public DateTime EndDate { set; get; }

        public int? VendorId { set; get; }
        public virtual Vendor Vendor { set; get; }
        public int? TenantId { get; set; }
        public virtual Tenant tenant { get; set; }
    }
}
