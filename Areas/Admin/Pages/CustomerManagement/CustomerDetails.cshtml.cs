using AssetProject.Data;
using AssetProject.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
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

namespace AssetProject.Areas.Admin.Pages.CustomerManagement
{
    [Authorize]
    public class CustomerDetailsModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        public Customer customer { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public CustomerDetailsModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);

                customer = _context.Customers.Find(id);
                if (customer == null)
                {
                    _toastNotification.AddErrorToastMessage("Something went error");
                    return RedirectToPage("CustomerList");

                }
                if (customer.TenantId != tenant.TenantId)
                {
                    return Redirect("../NotFound");
                }
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("CustomerList");
            }
            return Page();
        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions, int customerId)
        {
            var assetLeasing = _context.AssetLeasingDetails.Include(e=>e.AssetLeasing).Include(e=>e.Asset).Where(e => e.AssetLeasing.CustomerId == customerId).Select(e => new
            {
                e.AssetLeasing,
                e.AssetLeasing.StartDate,
                e.AssetLeasing.EndDate,
                e.Asset
            });
            return new JsonResult(DataSourceLoader.Load(assetLeasing, loadOptions));
        }
    }
}

