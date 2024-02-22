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
    public class SellAssetReportModel : PageModel
    {
        private readonly AssetContext _context;

        public SellAssetReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptSellAsset Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptSellAsset(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<SellAssetModel> ds = _context.AssetSellDetails.Include(e=>e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Select(i => new SellAssetModel
            {
                AssetCost = i.Asset.AssetCost,
                AssetDescription = i.Asset.AssetDescription,
                AssetSerialNo = i.Asset.AssetSerialNo,
                AssetTagId = i.Asset.AssetTagId,
               SaleAmount=i.SellAsset.SaleAmount,
               SaleDate=i.SellAsset.SaleDate,
               SellNotes=i.SellAsset.Notes,
               SoldTo=i.SellAsset.SoldTo,
               photo=i.Asset.Photo

            }).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }

            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId.Contains(filterModel.AssetTagId)).ToList();
            }
            if (filterModel.SoldTo != null)
            {
                ds = ds.Where(i => i.SoldTo.Contains(filterModel.SoldTo)).ToList();
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
                ds = ds.Where(i => i.SaleDate <= filterModel.ToDate && i.SaleDate >= filterModel.FromDate).ToList();
            }
            
            if (filterModel.ShowAll==false && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.SoldTo == null && filterModel.AssetTagId == null)
            {
                ds = null;
            }
           
            Report = new rptSellAsset(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
