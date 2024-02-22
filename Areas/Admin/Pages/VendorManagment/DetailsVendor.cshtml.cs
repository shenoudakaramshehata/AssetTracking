using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.VendorManagment
{
    [Authorize]
    public class DetailsVendorModel : PageModel
    {
        public Vendor Vendor { set; get; }
        AssetContext Context;
        public string VendorName;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DetailsVendorModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            UserManger = userManager;

        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);

            Vendor = Context.Vendors.Where(c => c.VendorId == id).FirstOrDefault();
            if (Vendor == null)
            {
                return Redirect("../NotFound");
            }
            if (Vendor.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }
            VendorName = Vendor.VendorTitle;
            return Page();
        }
    }
}
