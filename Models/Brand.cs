using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Brand
    {
        public int BrandId { set; get; }
        [Required]
        public string BrandTitle { set; get; }
        public int? TenantId { get; set; }

        public virtual Tenant tenant { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
