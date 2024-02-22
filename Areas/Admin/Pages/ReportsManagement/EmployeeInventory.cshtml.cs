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
    public class EmployeeInventoryModel : PageModel
    {
        private readonly AssetContext _context;

        public EmployeeInventoryModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }


        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptEmployeeInventory Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptEmployeeInventory(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = new List<AssetReportsModel>();
            var assets = _context.Assets.Where(e => e.TenantId == tenant.TenantId).Include(a => a.AssetStatus).Include(e => e.Item).Include(e => e.Vendor).Include(e => e.AssetMovementDetails).ThenInclude(e => e.AssetMovement).ToList();
            foreach (var asset in assets)
            {
                var t = _context.AssetMovementDetails.Where(a => a.AssetId == asset.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.EmpolyeeID;
                if (_context.AssetMovementDetails.Where(a => a.AssetId == asset.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.EmpolyeeID!=null)
                {
                    ds.Add(new AssetReportsModel
                    {
                        AssetCost = asset.AssetCost,
                        AssetPurchaseDate = asset.AssetPurchaseDate,
                        AssetSerialNo = asset.AssetSerialNo,
                        AssetTagId = asset.AssetTagId,
                        ItemTL = asset.Item.ItemTitle,
                        Photo = asset.Photo,
                        VendorTL = asset.Vendor.VendorTitle,
                        EmployeeFullName = _context.Employees.Find(t).FullName,
                        EmployeeId = t,
                        //EmployeeFullName = _context.Employees.Find(lastAssetMovement.AssetMovement.EmpolyeeID) == null ? null : _context.Employees.Find(lastAssetMovement.AssetMovement.EmpolyeeID).FullName,
                        AssetStatusTL = asset.AssetStatus.AssetStatusTitle,
                        //StatusId = asset.AssetStatusId,
                        TransactionDate= _context.AssetMovementDetails.Where(a => a.AssetId == asset.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.TransactionDate

                    }) ;
                }

            }
            if (filterModel.employeeId != null)
            {
                ds = ds.Where(i => i.EmployeeId == filterModel.employeeId).ToList();
            }
            //if (filterModel.nulstatusid != null)
            //{
            //    ds = ds.Where(i => i.StatusId == filterModel.nulstatusid).ToList();
            //}
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
                ds = ds.Where(i => (i.TransactionDate >= filterModel.FromDate || i.AssetPurchaseDate >= filterModel.FromDate) && (i.AssetPurchaseDate <= filterModel.ToDate || i.TransactionDate <= filterModel.ToDate)).ToList();
            }
            if ( filterModel.employeeId == null && filterModel.FromDate == null && filterModel.ToDate == null)
            {
                ds = null;
            }

            Report = new rptEmployeeInventory(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
