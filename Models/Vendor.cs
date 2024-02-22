using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Vendor
    {
       public int VendorId { set; get; }
        [Required]
       public string VendorTitle { set; get; }
        [Required, RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Phone { set; get; }
        [Required, RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Mobile { set; get; }
        [Required]
        [EmailAddress]
        public  string Email { set; get; }
        [Required]
        public string Website { set; get; }
        [Required]
        public  string ContactPersonName { set; get; }
        [Required]
        [EmailAddress]
        public string ContactPersonEmail { set; get; }
        [Required, RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string ContactPersonPhone { set; get; }
       public virtual ICollection<Contract> Cotracts { get; set; }
       public virtual ICollection<Purchase > Purchases { get; set; }
        public int? TenantId { get; set; }
        public virtual Tenant tenant { get; set; }

    }
}
