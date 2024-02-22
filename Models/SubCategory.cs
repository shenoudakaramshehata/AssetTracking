using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class SubCategory
    {
        public int SubCategoryId { set; get; }
        [Required]
        public string SubCategoryTitle { set; get; }
        [Required]
        public string SubCategoryDescription { set; get; }

        public virtual Category Category { set; get; }

        public int CategoryId { set; get; }
        public int? TenantId { get; set; }

        public virtual Tenant tenant { get; set; }

    }
}
