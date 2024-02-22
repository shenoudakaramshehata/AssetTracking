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
    public class EditTechnicianModel : PageModel
    {
            [BindProperty]
            public Technician technician { set; get; }
            AssetContext Context;
            private readonly IToastNotification _toastNotification;
            UserManager<ApplicationUser> UserManger;
            public Tenant tenant { set; get; }
            public EditTechnicianModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
            {
                Context = context;
                _toastNotification = toastNotification;
                UserManger = userManager;
            }
            public async Task<IActionResult> OnGet(int id)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = Context.Tenants.Find(user.TenantId);

                technician = Context.Technicians.Find(id);
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

            public IActionResult OnPost()
            {
              
                if (ModelState.IsValid)
                {
                    var UpdatedContract = Context.Technicians.Attach(technician);
                    UpdatedContract.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    try
                    {
                        Context.SaveChanges();
                        _toastNotification.AddSuccessToastMessage("Technician Edited successfully");
                        return RedirectToPage("/TechnicianManagement/DetailsTechnician", new { id = technician.TechnicianId });
                    }
                    catch (Exception e)
                    {
                        _toastNotification.AddErrorToastMessage("Something went wrong");
                        return RedirectToPage("/TechnicianManagement/EditTechnician", new { id = technician.TechnicianId });
                    }

                }
                return Page();
            }
        }
    }

