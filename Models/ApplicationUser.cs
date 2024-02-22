using Microsoft.AspNetCore.Identity;

namespace AssetProject.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Pic { set; get; }
        public int TenantId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        
    }
}
