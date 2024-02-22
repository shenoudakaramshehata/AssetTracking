using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.BrandManagment
{
    [Authorize]
    public class AddBrandModel : PageModel
    {
        [BindProperty]
        public Brand Brand { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AddBrandModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _toastNotification = toastNotification;
            Brand = new Brand();
            UserManger = userManager;

        }

    
        public void OnGet()
        {
        }

        public async Task <IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = Context.Tenants.Find(user.TenantId);
                Brand.TenantId = tenant.TenantId;
                Context.Brands.Add(Brand);
                try
                {

                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Brand Added successfully");
                    return RedirectToPage("/BrandManagment/BrandList");
                }
               catch(Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/BrandManagment/BrandList");
                }
            }
            return Page();
        }
    }
}
