using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Is Required"), MaxLength(50)]
        public string ItemTitle { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public string Description { get; set; }
        public virtual ICollection<PurchaseAsset> PurchaseAssets { get; set; }
        public int? TenantId { get; set; }

        public virtual Tenant tenant { get; set; }
    }
}
