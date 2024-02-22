using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public partial class Tenant
    {
        public int TenantId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public int? CountryId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        public virtual Country Country { get; set; }
    }
}
