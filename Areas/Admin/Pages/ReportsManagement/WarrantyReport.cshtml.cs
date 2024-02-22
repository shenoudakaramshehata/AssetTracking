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
using Microsoft.EntityFrameworkCore;


namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    [Authorize]
    public class WarrantyReportModel : PageModel
    {
        private readonly AssetContext _context;

        public WarrantyReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptWarranty Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            Report = new rptWarranty(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<WarrantyModel> ds = _context.AssetWarranties.Include(e => e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Select(i => new WarrantyModel
            {
                AssetCost = i.Asset.AssetCost,
                AssetSerialNo = i.Asset.AssetSerialNo,
                AssetTagId = i.Asset.AssetTagId,
                photo = i.Asset.Photo,
               ExpirationDate= i.ExpirationDate,
               Length=i.Length,
               WarrantyId= i.WarrantyId
            }).ToList();
            if (filterModel.FromDate != null && filterModel.ToDate == null)
            {
                ds = null;
            }
            if (filterModel.FromDate == null && filterModel.ToDate != null)
            {
                ds = null;
            }
            if (filterModel.FromDate != null && filterModel.ToDate != null)
            {
                ds = ds.Where(i => i.ExpirationDate <= filterModel.ToDate && i.ExpirationDate >= filterModel.FromDate).ToList();
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId == filterModel.AssetTagId).ToList();
            }
            if (filterModel.warrantyId != null)
            {
                ds = ds.Where(i => i.WarrantyId == filterModel.warrantyId).ToList();
            }

            if (filterModel.warrantyId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.AssetTagId == null)
            {
                ds = null;
            }
            
            Report = new rptWarranty(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
