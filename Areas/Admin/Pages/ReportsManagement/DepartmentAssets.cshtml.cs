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
    public class DepartmentAssetsModel : PageModel
    {
        public DepartmentAssetsModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        [BindProperty]
        public int AssetId { get; set; }
        [BindProperty]
        public FilterModel filterModel { get; set; }
        public rptDepartmentAssets Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AssetContext _context { get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptDepartmentAssets(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = _context.Assets.Where(e => e.TenantId == tenant.TenantId && e.AssetStatusId == 2).Include(a => a.AssetMovementDetails).ThenInclude(a => a.AssetMovement).ThenInclude(a => a.Department).Select(i => new AssetReportsModel
            {
                AssetCost = i.AssetCost,
                AssetSerialNo = i.AssetSerialNo,
                AssetTagId = i.AssetTagId,
                ItemTL = i.Item.ItemTitle,
                Photo = i.Photo,
                TransactionDate = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? null : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.TransactionDate,
                DepartmentId = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? 0 : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Department.DepartmentId,
                DepartmentTL = _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault() == null ? null : _context.AssetMovementDetails.Where(a => a.AssetId == i.AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault().AssetMovement.Department.DepartmentTitle

            }).ToList();

            if (filterModel.DepartmentId != null)
            {
                ds = ds.Where(i => i.DepartmentId == filterModel.DepartmentId).ToList();
            }
            if (filterModel.DepartmentId == null)
            {
                ds = new List<AssetReportsModel>();
            }
           
            Report = new rptDepartmentAssets(tenant);
            Report.DataSource = ds;
            return Page();

        }
    }
}
