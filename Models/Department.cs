using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public partial class Department
    {
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentTitle { get; set; }
        public int? TenantId { get; set; }

        public virtual Tenant tenant { get; set; }
    }
}
