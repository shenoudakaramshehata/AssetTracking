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
using NToastNotify;

namespace AssetProject.Areas.Admin.Pages.ItemManagement
{
    [Authorize]
    public class CreateItemModel : PageModel
    {
        [BindProperty]
        public Item Item { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public CreateItemModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _toastNotification = toastNotification;
            Item = new Item();
            UserManger = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Item.CategoryId == null)
                {
                    ModelState.AddModelError("", "Please Select Category");
                    return Page();
                }
                if (Item.BrandId == null)
                {
                    ModelState.AddModelError("", "Please Select Brand");
                    return Page();
                }
                try
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await UserManger.FindByIdAsync(userid);
                    tenant = Context.Tenants.Find(user.TenantId);
                    Item.TenantId = tenant.TenantId;
                    Context.Items.Add(Item);

                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Item Added successfully");
                    return RedirectToPage("/ItemManagement/ItemList");
                }
                catch(Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/ItemManagement/ItemList");
                }
            }
            return Page();
        }
    }
}
