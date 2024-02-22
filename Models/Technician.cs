using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Technician
    {
       public int TechnicianId { set; get; }
        [Required]
        public string FullName { set; get; }
        [Required]
        public string Mobile { set; get; }
        [Required]
        public string Address  { set; get; }
        public string Remarks { set; get; }
        public int? TenantId { set; get; }
        public virtual Tenant Tenant { set; get; }



        public ICollection<AssetRepair> AssetRepairs { set; get; }

        


    }
}
