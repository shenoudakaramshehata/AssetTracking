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

namespace AssetProject.Areas.Admin.Pages.InsuranceManagement
{
    [Authorize]
    public class EditInsuranceModel : PageModel
    {

        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Insurance insurance { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public EditInsuranceModel(AssetContext context,IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);

                insurance = _context.Insurances.Find(id);
                if (insurance == null)
                {
                    _toastNotification.AddErrorToastMessage("Something went error");
                    return RedirectToPage("InsuranceList");
                }
                if (insurance.TenantId != tenant.TenantId)
                {
                    return Redirect("../NotFound");
                }
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("InsuranceList");
            }
            return Page();

        }
        public IActionResult OnPost()
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
                _context.Entry(insurance).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Insurance Policy Update Successfuly");
                return RedirectToPage("/InsuranceManagement/DetailsInsurance", new { id = insurance.InsuranceId });

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("/InsuranceManagement/EditInsurance", new { id = insurance.InsuranceId});
            }
        }
    }
}
