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
    public class DisposeAssetReportModel : PageModel
    {
        
            private readonly AssetContext _context;

            public DisposeAssetReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

            [BindProperty]
            public FilterModel filterModel { get; set; }
            public int AssetId { get; set; }
            public rptDisposeAsset Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptDisposeAsset(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<DisposeModel> ds = _context.AssetDisposeDetails.Include(e=>e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Select(i => new DisposeModel
                {
                    AssetCost = i.Asset.AssetCost,
                    AssetDescription = i.Asset.AssetDescription,
                    AssetSerialNo = i.Asset.AssetSerialNo,
                    AssetTagId = i.Asset.AssetTagId,
                    DateDisposed = i.DisposeAsset.DateDisposed,
                    DisposeNotes = i.DisposeAsset.Notes,
                    DisposeTo = i.DisposeAsset.DisposeTo
                    ,photo=i.Asset.Photo
                }).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }
            if (filterModel.AssetTagId != null)
                {
                    ds = ds.Where(i => i.AssetTagId.Contains(filterModel.AssetTagId)).ToList();
                }
                if (filterModel.DisposeTo != null)
                {
                    ds = ds.Where(i => i.DisposeTo.Contains(filterModel.DisposeTo)).ToList();
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
                    ds = ds.Where(i => i.DateDisposed <= filterModel.ToDate && i.DateDisposed >= filterModel.FromDate).ToList();
                }
               
                if (filterModel.FromDate == null && filterModel.ShowAll == false && filterModel.ToDate == null&& filterModel.DisposeTo == null&& filterModel.AssetTagId == null)
                {
                    ds = null;
                }

           
            Report = new rptDisposeAsset(tenant);
            Report.DataSource = ds;
            return Page();
        }
        }
    }

