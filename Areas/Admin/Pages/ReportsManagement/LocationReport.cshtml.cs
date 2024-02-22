using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ReportModels;
using AssetProject.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    [Authorize]
    public class LocationReportModel : PageModel
    {
        private readonly AssetContext _context;

        public LocationReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }


        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptsite Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            tenant.Email = user.Email;
            tenant.Phone = user.PhoneNumber;
            Report = new rptsite(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            List<SiteModel> ds = _context.Locations.Select(i => new SiteModel
            {
                LocationId=i.LocationId,
                Address=i.Address,
                City=i.City,
                LocationTitle=i.LocationTitle, 
                PostalCode=i.PostalCode, 
                State= i.State

            }).ToList();
            if (filterModel.LocationId != null)
            {
                ds = ds.Where(i => i.LocationId == filterModel.LocationId).ToList();
            }

            if (filterModel.LocationId == null)
            {
                ds = null;
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            tenant.Email = user.Email;
            tenant.Phone = user.PhoneNumber;
            Report = new rptsite(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
