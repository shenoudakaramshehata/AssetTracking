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

namespace AssetProject.Areas.Admin.Pages.TechnicianManagement
{
    [Authorize]

    public class AddTechnicianModel : PageModel
    {
        [BindProperty]
        public Technician technician { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AddTechnicianModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _toastNotification = toastNotification;
            technician = new Technician();
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
                technician.TenantId = tenant.TenantId;
                Context.Technicians.Add(technician);
                await Context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Technicians Added successfully");
                return RedirectToPage("/TechnicianManagement/TechnicianList");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToPage("/TechnicianManagement/TechnicianList");
            }
        }
    }
}
