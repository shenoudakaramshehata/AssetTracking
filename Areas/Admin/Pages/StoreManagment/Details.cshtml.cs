using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssetProject.Areas.Admin.Pages.StoreManagment
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly AssetProject.Data.AssetContext _context;

        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DetailsModel(AssetProject.Data.AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        public Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Store = _context.Stores.Find(id);

            if (Store == null)
            {
                return Redirect("../NotFound");
            }
            if (Store.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }

            return Page();
        }
    }
}
