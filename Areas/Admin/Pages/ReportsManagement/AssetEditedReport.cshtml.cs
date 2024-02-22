using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class AssetEditedReportModel : PageModel
    {
        private readonly AssetContext _context;
        public AssetEditedReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public rptAssetEdited Report { get; set; }
        static List<AssetReportsModel> ShowAllList = new List<AssetReportsModel>();

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            tenant.Email = user.Email;
            tenant.Phone = user.PhoneNumber;
            Report = new rptAssetEdited(tenant);
            return Page();

        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = _context.AssetLogs.Include(e=>e.Asset).Where(l =>l.Asset.TenantId==tenant.TenantId &&l.ActionLogId == 19).Select(i => new AssetReportsModel
            {
                AssetCost = i.Asset.AssetCost,
                AssetPurchaseDate = i.Asset.AssetPurchaseDate,
                AssetSerialNo = i.Asset.AssetSerialNo,
                AssetStatusTL = i.Asset.AssetStatus.AssetStatusTitle,
                AssetTagId = i.Asset.AssetTagId,
                CategoryTL = i.Asset.Item.Category.CategoryTIAR,  
                ItemTL = i.Asset.Item.ItemTitle,
                Photo = i.Asset.Photo,
                LocationTL = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? null : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Location.LocationTitle,
                DepartmentTL = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? null : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Department.DepartmentTitle,
                LocationId = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? 0 : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Location.LocationId,
                DepartmentId = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? 0 : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId && a.Asset.AssetStatusId == 2).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Department.DepartmentId,
                CategoryId = i.Asset.Item.CategoryId,
                LogActionDate = i.ActionDate



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
            if (filterModel.FromDate != null&& filterModel.ToDate != null)
            {
                ds = ds.Where(i => (i.LogActionDate.Value.Date) > filterModel.FromDate.Value.Date&& i.LogActionDate.Value.Date< filterModel.ToDate.Value.Date).ToList();
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId.Contains(filterModel.AssetTagId)).ToList();
            }
            //if (filterModel.LocationId != null)
            //{
            //    ds = ds.Where(i => i.LocationId == filterModel.LocationId).ToList();
            //}
            //if (filterModel.DepartmentId != null)
            //{
            //    ds = ds.Where(i => i.DepartmentId == filterModel.DepartmentId).ToList();
            //}
            //if (filterModel.CategoryId != null)
            //{
            //    ds = ds.Where(i => i.CategoryId == filterModel.CategoryId).ToList();
            //}

            if (filterModel.AssetTagId == null && filterModel.OnDay == null&& filterModel.ShowAll == false)
            {
                ds = new List<AssetReportsModel>();
            }

           
            Report = new rptAssetEdited(tenant);
            Report.DataSource = ds;

            return Page();

        }
    }
}

