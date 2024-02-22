using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AssetProject.Models
{
    public partial class Location
    {
        
        public Location()
        {
            InverseLocationParent = new HashSet<Location>();
        }

        public int LocationId { get; set; }
        public int? LocationParentId { get; set; }
        [Required]
        public string LocationTitle { get; set; }
        public int? CountryId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public int? TenantId { get; set; }

        public virtual Tenant tenant { get; set; }

        [Required]
        public string BarCode { get; set; }
        [Required]
        public string LocationLatitude{ get; set; }
        [Required]
        public string LocationLangtiude { get; set; }
        [JsonIgnore]
        public virtual Location LocationParent { get; set; }
        public virtual ICollection<Location> InverseLocationParent { get; set; }

    }
}
