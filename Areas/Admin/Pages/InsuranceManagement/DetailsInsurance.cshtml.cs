using AssetProject.Data;
using AssetProject.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.InsuranceManagement
{
    [Authorize]
    public class DetailsInsuranceModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
            public Insurance insurance { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
          UserManager<ApplicationUser> UserManger;
          public Tenant tenant { set; get; }

        public DetailsInsuranceModel(AssetContext context,IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGet(int? id)
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

        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions, int InsuranceId)
        {
            var assetcontracts = _context.AssetsInsurances.Where(e => e.InsuranceId == InsuranceId).Select(e => new
            {
                e.Asset
            });
            return new JsonResult(DataSourceLoader.Load(assetcontracts, loadOptions));
        }
    }
}
