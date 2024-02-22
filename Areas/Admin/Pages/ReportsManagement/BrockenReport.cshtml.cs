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
    public class BrockenReportModel : PageModel
    {
        private readonly AssetContext _context;

        public BrockenReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptAssetBrocken Report { get; set; }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;

        static List<BrockenModel> ShowAllList = new List<BrockenModel>();
        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptAssetBrocken(tenant);
            return Page();

        }

        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<BrockenModel> ds = _context.AssetBrokenDetails.Include(e=>e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Select(i => new BrockenModel
            {
                AssetCost = i.Asset.AssetCost,
                AssetDescription = i.Asset.AssetDescription,
                AssetSerialNo = i.Asset.AssetSerialNo,
                AssetTagId = i.Asset.AssetTagId,
                AssetId = i.AssetId,
                Photo = i.Asset.Photo,
                CategoryTL = i.Asset.Item.Category.CategoryTIAR,
               
                CategoryId = i.Asset.Item.CategoryId,
                ItemTL=i.Asset.Item.ItemTitle,
                StoreTL=i.Asset.Store.StoreTitle,
                AssetBrokenId = i.AssetBrokenId,
                DateBroken = i.AssetBroken.DateBroken,
                Notes = i.AssetBroken.Notes,

            }).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }
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
                ds = ds.Where(i => (i.DateBroken.Date) >= filterModel.FromDate.Value.Date && i.DateBroken.Date <= filterModel.ToDate.Value.Date).ToList();
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId.Contains(filterModel.AssetTagId)).ToList();
            }
          
            
            if (filterModel.AssetTagId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.ShowAll == false)
            {
                ds = new List<BrockenModel>();
            }
            
            Report = new rptAssetBrocken(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
