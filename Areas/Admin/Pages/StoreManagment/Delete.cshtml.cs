using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetProject.Data;
using AssetProject.Models;
using NToastNotify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssetProject.Areas.Admin.Pages.StoreManagment
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public DeleteModel( AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.toastNotification = toastNotification;
            UserManger = userManager;
        }

        [BindProperty]
        public Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            Store = await _context.Stores.FirstOrDefaultAsync(m => m.StoreId == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Store = await _context.Stores.FindAsync(id);

            if (Store != null)
            {
                _context.Stores.Remove(Store);
                try { 
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Store Deleted successfully");
                return RedirectToPage("/StoreManagment/List");
                }
                catch (Exception e)
                {
                    toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/StoreManagment/Delete",new { id=Store.StoreId});
                }
            }

            toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("/StoreManagment/List");
        }
    }
}
