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


namespace AssetProject.Areas.Admin.Pages.TechnicianManagement
{
    [Authorize]

    public class DeleteTechnicianModel : PageModel
    {
        public Technician technician { set; get; }
        AssetContext Context;
        //public string VendorName;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DeleteTechnicianModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
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

            technician = Context.Technicians.Where(c => c.TechnicianId == id).FirstOrDefault();
            if (technician == null)
            {
                return Redirect("../NotFound");
            }

            if (technician.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            technician = Context.Technicians.Find(id);
            if (technician != null)
            {

                Context.Technicians.Remove(technician);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Technician Deleted successfully");
                    return RedirectToPage("/TechnicianManagement/TechnicianList");
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/TechnicianManagement/DeleteTechnician", new { id = technician.TechnicianId });
                }
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("/VendorManagment/VendorList");


        }
    }
}
