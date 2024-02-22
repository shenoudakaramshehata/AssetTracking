using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssetProject.Areas.Admin.Pages.StoreManagment
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AssetProject.Data.AssetContext _context;
        private readonly IToastNotification toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public EditModel(AssetProject.Data.AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("Store Edited successfully");
                return RedirectToPage("/StoreManagment/Details",new {id=Store.StoreId });
            }
            catch (Exception e)
            {
                toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToPage("/StoreManagment/Edit", new { id = Store.StoreId });
            }

        }
    }
}
