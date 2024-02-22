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
    public class StoreOutcomingModel : PageModel
    {
        private readonly AssetContext _context;

        public StoreOutcomingModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }


        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptOutcomingStoreHistory Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptOutcomingStoreHistory(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = new List<AssetReportsModel>();
            var assets = _context.Assets.Where(e=>e.TenantId==tenant.TenantId).Include(e => e.Store).Include(e => e.Item).Include(e => e.Vendor).Include(e => e.AssetMovementDetails).ThenInclude(e => e.AssetMovement).ThenInclude(e => e.Store).ToList();
            foreach (var asset in assets)
            {
                if (asset.AssetMovementDetails.Count() > 0)
                {

                    var checkoutMovements = asset.AssetMovementDetails.Where(e => e.AssetMovement.AssetMovementDirectionId == 1).ToList();
                    foreach (var movement in checkoutMovements)
                    {
                        ds.Add(new AssetReportsModel
                        {
                            AssetCost = asset.AssetCost,
                            AssetSerialNo = asset.AssetSerialNo,
                            AssetTagId = asset.AssetTagId,
                            ItemTL = asset.Item.ItemTitle,
                            Photo = asset.Photo,
                            StoreTL = movement.AssetMovement.Store==null?null: movement.AssetMovement.Store.StoreTitle,
                            VendorTL = asset.Vendor.VendorTitle,
                            StoreId = movement.AssetMovement.StoreId,
                            TransactionDate = movement.AssetMovement.TransactionDate
                        });
                    }
                }
            }
            if (filterModel.StoreId != null)
            {
                ds = ds.Where(i => i.StoreId == filterModel.StoreId).ToList();
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
                ds = ds.Where(i => i.TransactionDate >= filterModel.FromDate  && i.TransactionDate <= filterModel.ToDate).ToList();
            }
            if (filterModel.StoreId == null && filterModel.FromDate == null && filterModel.ToDate == null)
            {
                ds = null;
            }
          
            Report = new rptOutcomingStoreHistory(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
