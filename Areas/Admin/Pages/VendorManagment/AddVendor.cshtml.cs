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

namespace AssetProject.Areas.Admin.Pages.VendorManagment
{
    [Authorize]
    public class AddVendorModel : PageModel
    {
        [BindProperty]
        public Vendor Vendor { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AddVendorModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
           _toastNotification = toastNotification;
            Vendor = new Vendor();
            UserManger = userManager;
        }
    

        public IActionResult OnGet()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = Context.Tenants.Find(user.TenantId);
                Vendor.TenantId = tenant.TenantId;
                Context.Vendors.Add(Vendor);
                await Context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Vendor Added successfully");
                return RedirectToPage("/VendorManagment/VendorList");
            }
            catch(Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToPage("/VendorManagment/VendorList");
            }
        }
    }
}
