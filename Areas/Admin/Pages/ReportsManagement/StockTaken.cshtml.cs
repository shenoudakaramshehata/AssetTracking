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
    public class StockTakenModel : PageModel
    {
        private readonly AssetContext _context;

        public StockTakenModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }


        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public RptStockTaken Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new RptStockTaken(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = _context.Assets.Where(e=>e.TenantId==tenant.TenantId&&e.AssetStatusId==1).Include(e => e.Item).Include(e => e.Vendor).Include(e=>e.Store).Include(e=>e.AssetMovementDetails).ThenInclude(e => e.AssetMovement).ThenInclude(e=>e.Store).Select(i => new AssetReportsModel
            {
                AssetCost = i.AssetCost,
                AssetPurchaseDate = i.AssetPurchaseDate,
                AssetSerialNo = i.AssetSerialNo,
                AssetTagId = i.AssetTagId,
                ItemTL = i.Item.ItemTitle,
                Photo = i.Photo,
                StoreTL = i.AssetMovementDetails.Count()>0?i.AssetMovementDetails.OrderByDescending(e=>e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Store.StoreTitle : i.Store.StoreTitle,
                VendorTL = i.Vendor.VendorTitle,
                StoreId = i.AssetMovementDetails.Count() > 0 ? i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.StoreId : i.StoreId

            }).ToList();
            
            if (filterModel.StoreId != null)
            {
                ds = ds.Where(i => i.StoreId == filterModel.StoreId).ToList();
            }
            else
            {
                ds = null;
            }
          
            Report = new RptStockTaken(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
