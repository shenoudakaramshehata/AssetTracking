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

namespace AssetProject.Areas.Admin.Pages.Reports
{
    [Authorize]
    public class PrintAssetCheckOutModel : PageModel
    {
        private readonly AssetContext _context;

        public PrintAssetCheckOutModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rtpAssetCheckOut Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rtpAssetCheckOut(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetCheckOutList> ds = new List<AssetCheckOutList>();
            var checkedOutAssets = _context.Assets.Where(e =>e.TenantId==tenant.TenantId&&e.AssetStatusId == 2).Include(a => a.AssetMovementDetails).ThenInclude(a => a.AssetMovement);
            foreach (var asset in checkedOutAssets)
            {
                var lastAssetMovement = asset.AssetMovementDetails.OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                ds.Add(new AssetCheckOutList()
                {
                    TransactionDate = lastAssetMovement.AssetMovement.TransactionDate,
                    EmployeeFullN = _context.Employees.Find(lastAssetMovement.AssetMovement.EmpolyeeID) == null ? null : _context.Employees.Find(lastAssetMovement.AssetMovement.EmpolyeeID).FullName,
                    LocationTl = _context.Locations.Find(lastAssetMovement.AssetMovement.LocationId).LocationTitle,
                    DepartmentTl = _context.Departments.Find(lastAssetMovement.AssetMovement.DepartmentId).DepartmentTitle,
                    Photo = lastAssetMovement.Asset.Photo,
                    StoreTl = _context.Stores.Find(lastAssetMovement.AssetMovement.StoreId)==null?null: _context.Stores.Find(lastAssetMovement.AssetMovement.StoreId).StoreTitle,
                    ActionTypeTl = _context.ActionTypes.Find(lastAssetMovement.AssetMovement.ActionTypeId).ActionTypeTitle,
                    AssetPurchaseDate = lastAssetMovement.Asset.AssetPurchaseDate,
                    AssetSerialNo = lastAssetMovement.Asset.AssetSerialNo,
                    AssetTagId = lastAssetMovement.Asset.AssetTagId,
                    AssetMovementId = lastAssetMovement.AssetMovementId,
                    EmployeeId = lastAssetMovement.AssetMovement.EmpolyeeID,
                    LocationId = lastAssetMovement.AssetMovement.LocationId,
                    StoreId = lastAssetMovement.AssetMovement.StoreId,
                    DepartmentId = lastAssetMovement.AssetMovement.DepartmentId

                });
            }
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }
            if (filterModel.employeeId!=null)
            {
                ds = ds.Where(e => e.EmployeeId == filterModel.employeeId).ToList();
            }
            if (filterModel.DepartmentId != null)
            {
                ds = ds.Where(e => e.DepartmentId== filterModel.DepartmentId).ToList();
            }
            if (filterModel.LocationId!=null)
            {
                ds = ds.Where(e => e.LocationId == filterModel.LocationId).ToList();
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId == filterModel.AssetTagId).ToList();
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
                ds = ds.Where(i => i.TransactionDate <= filterModel.ToDate && i.TransactionDate >= filterModel.FromDate).ToList();
            }
          
            if (filterModel.ShowAll == false&&filterModel.DepartmentId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.LocationId == null && filterModel.AssetTagId == null && filterModel.employeeId == null)
            {
                ds = null;
            }

           
            Report = new rtpAssetCheckOut(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
