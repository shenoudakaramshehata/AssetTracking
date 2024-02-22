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
    public class MaintainanceReportModel : PageModel
    {
        private readonly AssetContext _context;

        public MaintainanceReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptMaintainance Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
           
            Report = new rptMaintainance(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<MaintainanceModel> ds = _context.AssetMaintainances.Include(e=>e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Include(e=>e.AssetMaintainanceFrequency).Include(e=>e.MaintainanceStatus).Include(e=>e.Technician).Select(i => new MaintainanceModel
            {
                AssetCost = i.Asset.AssetCost,
                AssetSerialNo = i.Asset.AssetSerialNo,
                AssetTagId = i.Asset.AssetTagId,
                ScheduleDate = i.ScheduleDate,
                photo = i.Asset.Photo,
                AssetMaintainanceFrequencyTl = i.AssetMaintainanceFrequency.AssetMaintainanceFrequencyTitle,
                AssetMaintainanceRepairesCost = i.AssetMaintainanceRepairesCost,
                AssetMaintainanceTitle = i.AssetMaintainanceTitle,
                MaintainanceStatusTL = i.MaintainanceStatus.MaintainanceStatusTitle,
                TechnicianName = i.Technician.FullName,
                TechnicianId = i.TechnicianId,
                AssetMaintainanceDateCompleted=i.AssetMaintainanceDateCompleted,
                MaintStatusId=i.MaintainanceStatusId
            }).ToList();

            if (filterModel.radiobtn != null)
            {
            
                if (filterModel.radiobtn == "Schedule Date"&& filterModel.FromDate != null && filterModel.ToDate != null)
                {
                    ds = ds.Where(i => i.ScheduleDate <= filterModel.ToDate && i.ScheduleDate >= filterModel.FromDate).ToList();
                }
                else if (filterModel.radiobtn == "Completed Date" && filterModel.FromDate != null && filterModel.ToDate != null)
                {
                    ds = ds.Where(i => i.AssetMaintainanceDateCompleted <= filterModel.ToDate && i.AssetMaintainanceDateCompleted >= filterModel.FromDate).ToList();
                }
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId == filterModel.AssetTagId).ToList();
            }
            if (filterModel.TechnicianId != null)
            {
                ds = ds.Where(i => i.TechnicianId == filterModel.TechnicianId).ToList();
            }
            if (filterModel.MaintStatusId != null)
            {
                ds = ds.Where(i => i.MaintStatusId == filterModel.MaintStatusId).ToList();
            }
            if (filterModel.FromDate != null && filterModel.ToDate == null)
            {
                ds = null;
            }
            if (filterModel.FromDate == null && filterModel.ToDate != null)
            {
                ds = null;
            }
            if ( filterModel.FromDate != null && filterModel.ToDate != null && filterModel.radiobtn == null)
            {
                ds = ds.Where(i => i.ScheduleDate <= filterModel.ToDate && i.ScheduleDate >= filterModel.FromDate).ToList();
            }
            if (filterModel.radiobtn == null  && filterModel.MaintStatusId == null && filterModel.FromDate == null && filterModel.ToDate == null&& filterModel.TechnicianId == null&& filterModel.AssetTagId == null)
            {
                ds = null;
            }
            if (filterModel.radiobtn != null && filterModel.MaintStatusId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.TechnicianId == null && filterModel.AssetTagId == null)
            {
                ds = null;
            }
           
           
            Report = new rptMaintainance(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
