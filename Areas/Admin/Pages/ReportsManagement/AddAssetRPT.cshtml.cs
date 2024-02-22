using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ReportModels;
using AssetProject.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    [Authorize]
    public class AddAssetRPTModel : PageModel
    {

        public AddAssetRPTModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public rptAddAsset Report { get; set; }
        public AssetContext _context { get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptAddAsset(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = _context.Assets.Where(e=>e.TenantId==tenant.TenantId).Select(a => new AssetReportsModel
            {
             AssetTagId=a.AssetTagId,
             AssetSerialNo=a.AssetSerialNo,
             AssetPurchaseDate=a.AssetPurchaseDate,
             AssetStatusTL=a.AssetStatus.AssetStatusTitle,
             ItemTL=a.Item.ItemTitle,
             VendorTL=a.Vendor.VendorTitle,
             ItemId=a.ItemId,
             VendorId=a.VendorId,
             CategoryTL=a.Item.Category.CategoryTIAR,
             CategoryId = a.Item.CategoryId,
             AssetCost=a.AssetCost,
             Photo=a.Photo
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
                ds = ds.Where(i => i.AssetPurchaseDate <= filterModel.ToDate && i.AssetPurchaseDate >= filterModel.FromDate).ToList();
            }
            if (filterModel.CategoryId != null)
            {
                ds = ds.Where(i => i.CategoryId == filterModel.CategoryId).ToList();
            }
            if (filterModel.VendorId != null)
            {
                ds = ds.Where(i => i.VendorId == filterModel.VendorId).ToList();
            }
           
            if (filterModel.ItemId != null)
            {
                ds = ds.Where(i => i.ItemId == filterModel.ItemId).ToList();
            }
            if ( filterModel.LocationId == null && filterModel.DepartmentId == null && filterModel.CategoryId == null && filterModel.FromDate == null && filterModel.ToDate == null&& filterModel.ItemId == null && filterModel.VendorId == null && filterModel.ShowAll == false)  
            {
                ds = null;
            }
           
            Report = new rptAddAsset(tenant);
            Report.DataSource = ds;
            return Page();

        }
    }
}
