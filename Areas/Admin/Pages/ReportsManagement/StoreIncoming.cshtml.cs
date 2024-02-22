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
    public class StoreIncomingModel : PageModel
    {
        private readonly AssetContext _context;

        public StoreIncomingModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }


        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptStoreIncomeHistory Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptStoreIncomeHistory(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = new List<AssetReportsModel>();
            var assets = _context.Assets.Where(e=>e.TenantId==tenant.TenantId).Include(e => e.Store).Include(e=>e.Item).Include(e => e.Vendor).Include(e => e.AssetMovementDetails).ThenInclude(e => e.AssetMovement).ThenInclude(e => e.Store).ToList();
            foreach (var asset in assets)
            {
                ds.Add(new AssetReportsModel
                {
                    AssetCost = asset.AssetCost,
                    AssetPurchaseDate = asset.AssetPurchaseDate,
                    AssetSerialNo = asset.AssetSerialNo,
                    AssetTagId = asset.AssetTagId,
                    ItemTL = asset.Item.ItemTitle,
                    Photo = asset.Photo,
                    StoreTL = asset.Store.StoreTitle,
                    VendorTL = asset.Vendor.VendorTitle,
                    StoreId = asset.Store.StoreId,
                });
                if (asset.AssetMovementDetails.Count() > 0)
                {

                    var checkinMovements = asset.AssetMovementDetails.Where(e => e.AssetMovement.AssetMovementDirectionId == 2).ToList();
                    foreach (var movement in checkinMovements)
                    {
                        ds.Add(new AssetReportsModel
                        {
                            AssetCost = asset.AssetCost,
                            AssetSerialNo = asset.AssetSerialNo,
                            AssetTagId = asset.AssetTagId,
                            ItemTL = asset.Item.ItemTitle,
                            Photo = asset.Photo,
                            StoreTL = movement.AssetMovement.Store.StoreTitle,
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
                ds = ds.Where(i =>( i.TransactionDate >= filterModel.FromDate || i.AssetPurchaseDate >= filterModel.FromDate) && (i.AssetPurchaseDate <= filterModel.ToDate || i.TransactionDate <= filterModel.ToDate)).ToList();
            }
            if (filterModel.StoreId == null && filterModel.FromDate == null && filterModel.ToDate == null)
            {
                ds = null;
            }
          
            Report = new rptStoreIncomeHistory(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
