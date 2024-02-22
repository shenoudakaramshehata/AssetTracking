using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.BrandManagment
{
    [Authorize]
    public class BrandDetailsModel : PageModel
    {
        public Brand Brand { set; get; }
        AssetContext Context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public BrandDetailsModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            UserManger = userManager;
        }
        public async Task <IActionResult> OnGet(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);
            Brand = Context.Brands.Find(id);
            if (Brand == null)
            {
                return Redirect("../NotFound");
            }
            if (Brand.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }

            return Page();
        }
    }
}
