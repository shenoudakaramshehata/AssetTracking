using System.Collections.Generic;

namespace AssetProject.Models
{
    public partial class Country
    {
        public Country()
        {
            Tenants = new HashSet<Tenant>();
        }

        public int CountryId { get; set; }
        public string CountryTitle { get; set; }
       

        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}
