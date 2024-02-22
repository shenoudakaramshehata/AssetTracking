using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.InsuranceManagement
{
    [Authorize]

    public class DeleteInsuranceModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [BindProperty]
        public Insurance insurance { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DeleteInsuranceModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGet(int ?id)
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
                StartDate = insurance.StartDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                EndDate = insurance.StartDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("InsuranceList");
            }
            return Page();
        }
        public IActionResult OnPost(int id)
        {
            insurance = _context.Insurances.Find(id);
            if (insurance != null)
            {
                try
                {
                    _context.Insurances.Remove(insurance);
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Insurance Policy Deleted Successfuly");
                    return RedirectToPage("/InsuranceManagement/InsuranceList");
                }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("Something went error");
                    return RedirectToPage("/InsuranceManagement/DeleteInsurance", new { id = insurance.InsuranceId });
                }
            }
            _toastNotification.AddErrorToastMessage("Something went error");
            return RedirectToPage("/InsuranceManagement/InsuranceList");
        }
    }
    
}
