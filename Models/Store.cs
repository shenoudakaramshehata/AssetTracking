using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage ="Title must be between 3 to 50 character..")]
        public string StoreTitle { get; set; }
        [Required]
        public string Address { get; set; }
        [Phone]
        public string Tele { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public int? TenantId { get; set; }
        public virtual Tenant tenant { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }

    }
}
