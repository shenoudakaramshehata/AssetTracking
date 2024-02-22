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

namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    [Authorize]
    public class AssetStatusReportModel : PageModel
    {
        public AssetStatusReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public rptAssetStatus Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public Asset asset { set; get; }

        public AssetContext _context { get; }
        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            tenant.Email = user.Email;
            tenant.Phone = user.PhoneNumber;
            Report = new rptAssetStatus(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetReportsModel> ds = _context.Assets.Where(e=>e.TenantId==tenant.TenantId).Select(i => new AssetReportsModel
            {
                AssetCost = i.AssetCost,
                AssetSerialNo = i.AssetSerialNo,
                AssetStatusTL = i.AssetStatus.AssetStatusTitle,
                AssetTagId = i.AssetTagId,
                ItemTL = i.Item.ItemTitle,
                Photo = i.Photo,
                StoreTL = i.Store.StoreTitle,
                VendorTL = i.Vendor.VendorTitle,
                DepreciationMethodTL = i.DepreciationMethod.DepreciationMethodTitle,
                StatusId=i.AssetStatusId,
            }).ToList();
            

            if (filterModel.StatusId != 0)
            {
                ds = ds.Where(i => i.StatusId == filterModel.StatusId).ToList();
            }
            if (filterModel.StatusId == 0)
            {
                ds = new List<AssetReportsModel>();
            }

            tenant.Email = user.Email;
            tenant.Phone = user.PhoneNumber;
            Report = new rptAssetStatus(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
    }
