using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.VendorManagment
{
    [Authorize]
    public class DeleteVendorModel : PageModel
    {
     
        public Vendor Vendor { set; get; }
        AssetContext Context;
        //public string VendorName;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DeleteVendorModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _toastNotification = toastNotification;
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

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            Vendor = Context.Vendors.Find(id);
            if (Vendor != null)
            {

                Context.Vendors.Remove(Vendor);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Vendor Deleted successfully");
                    return RedirectToPage("/VendorManagment/VendorList");
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/VendorManagment/DeleteVendor",new { id=Vendor.VendorId});
                }
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("/VendorManagment/VendorList");


        }
    }
}
