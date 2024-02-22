using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetProject.Areas.Admin.Pages.ItemManagement
{
    [Authorize]
    public class DetailsItemModel : PageModel
    {
        public Item Item { set; get; }
        AssetContext Context;
        public string BrandName;
        public string CategoryName;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DetailsItemModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);

            Item = Context.Items.Where(c => c.ItemId == id).Include(c => c.Brand).Include(c => c.Category).FirstOrDefault();
            if (Item == null)
            {
                return Redirect("../NotFound");
            }
            if (Item.Brand != null && Item.Category != null)
            {
                BrandName = Item.Brand.BrandTitle;
                CategoryName = Item.Category.CategoryTIAR;

            }
            if (Item.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }


            return Page();
        }
    }
}
