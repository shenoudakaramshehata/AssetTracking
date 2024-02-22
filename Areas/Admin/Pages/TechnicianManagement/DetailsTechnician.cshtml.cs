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


namespace AssetProject.Areas.Admin.Pages.TechnicianManagement
{
    [Authorize]

    public class DetailsTechnicianModel : PageModel
    {
        public Technician technician { set; get; }
        AssetContext Context;
        public string technicianName;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DetailsTechnicianModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            UserManger = userManager;

        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);

            technician = Context.Technicians.Where(c => c.TechnicianId == id).FirstOrDefault();
            if (technician == null)
            {
                return Redirect("../NotFound");
            }
            if (technician.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }
            technicianName = technician.FullName;
            return Page();
        }
    }
}
