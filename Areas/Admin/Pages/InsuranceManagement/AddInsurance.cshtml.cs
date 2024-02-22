using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.InsuranceManagement
{
    [Authorize]
    public class AddInsuranceModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Insurance insurance { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public AddInsuranceModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            insurance = new Insurance();
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            if (insurance.EndDate <= insurance.StartDate)
            {
                ModelState.AddModelError("", "EndDate mustbe greater than StartDate  ");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();
                try
                {
                     var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                     var user = await UserManger.FindByIdAsync(userid);
                     tenant = _context.Tenants.Find(user.TenantId);
                     insurance.TenantId = tenant.TenantId;
                     _context.Insurances.Add(insurance);
                     _context.SaveChanges();
                     _toastNotification.AddSuccessToastMessage("Insurance Policy Created Successfully");
                }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }

            return RedirectToPage("InsuranceList");
        }
    }
}
