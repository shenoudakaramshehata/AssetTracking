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
    public class AssetHistoryModel : PageModel
    {
        public AssetHistoryModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        [BindProperty]
        public FilterModel filterModel { get; set; }
        public AssetHistory Report { get; set; }
        public AssetContext _context { get; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new AssetHistory(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<TransactionHistoryRM> ds = _context.AssetLogs.Include(e=>e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Select(a => new TransactionHistoryRM
            {
                ActionDate = a.ActionDate,
                Remark = a.Remark,
                AssetLogId = a.AssetLogId,
                ActionLogTitle = a.ActionLog.ActionLogTitle,
                AssetId = a.AssetId,
                ActionLogId = a.ActionLogId,
                AssetCost = a.Asset.AssetCost,
                AssetDescription = a.Asset.AssetDescription,
                AssetSerialNo = a.Asset.AssetSerialNo,
                AssetTagId = a.Asset.AssetTagId,
                photo = a.Asset.Photo
            }).ToList();

            if (filterModel.radiobtn != null)
            {
                if (filterModel.radiobtn== "Tag Id")
                {
                    ds = ds.Where(i => i.AssetTagId == filterModel.AssetSerialNo).ToList();
                }
                else if (filterModel.radiobtn == "Serial Number")
                { 
                    ds = ds.Where(i => i.AssetSerialNo == filterModel.AssetSerialNo).ToList();
                }
            }
            if (filterModel.radiobtn == null&&filterModel.AssetSerialNo==null)
            {
                ds = null;
            }
            filterModel.AssetSerialNo = null;
           
            Report = new AssetHistory(tenant);
            Report.DataSource = ds;
            return Page();

        }
    }
}
