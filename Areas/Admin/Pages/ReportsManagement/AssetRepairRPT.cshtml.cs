using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ReportModels;
using AssetProject.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    [Authorize]
    public class AssetRepairRPTModel : PageModel
    {
        private readonly AssetContext _context;

        public AssetRepairRPTModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;

        }
        public rptAssetRepair Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptAssetRepair(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            //    List<AssetRepairRM> ds = _context.AssetRepairDetails.Select(a => new AssetRepairRM
            //    {
            //        AssetId=a.AssetId,
            //        AssetTagId=a.Asset.AssetTagId,
            //        AssetSerialNo=a.Asset.AssetSerialNo,
            //        AssetCost=a.Asset.AssetCost,
            //        AssetDescription=a.Asset.AssetDescription,
            //        AssetRepairId=a.AssetRepairId,
            //        ScheduleDate=a.AssetRepair.ScheduleDate,
            //        CompletedDate=a.AssetRepair.CompletedDate,
            //        RepairCost=a.AssetRepair.RepairCost,
            //        Notes=a.AssetRepair.Notes,
            //        TechnicianId=a.AssetRepair.TechnicianId,
            //        TechnicianName=a.AssetRepair.Technician.FullName,
            //        photo=a.Asset.Photo

            //    }).ToList();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<AssetRepairRM> ds = _context.Assets.Include(i => i.AssetRepairDetails).ThenInclude(i => i.AssetRepair).ThenInclude(i=>i.Technician).Where(c =>c.TenantId==tenant.TenantId &&c.AssetStatusId ==3).Select(i => new AssetRepairRM
         {
             AssetId = i.AssetId,
             AssetCost = i.AssetCost,
             AssetSerialNo = i.AssetSerialNo,
             AssetTagId = i.AssetTagId,
             photo = i.Photo,
             ScheduleDate = i.AssetRepairDetails.OrderByDescending(e => e.AssetRepairDetailsId).FirstOrDefault().AssetRepair.ScheduleDate,
             CompletedDate = i.AssetRepairDetails.OrderByDescending(e => e.AssetRepairDetailsId).FirstOrDefault().AssetRepair.CompletedDate,
             RepairCost = i.AssetRepairDetails.OrderByDescending(e => e.AssetRepairDetailsId).FirstOrDefault().AssetRepair.RepairCost,
             TechnicianId = i.AssetRepairDetails.OrderByDescending(e => e.AssetRepairDetailsId).FirstOrDefault().AssetRepair.Technician.TechnicianId,
             TechnicianName = i.AssetRepairDetails.OrderByDescending(e => e.AssetRepairDetailsId).FirstOrDefault().AssetRepair.Technician.FullName

         }).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }

            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId == filterModel.AssetTagId).ToList();
            }
            if (filterModel.TechnicianId != null)
            {
                ds = ds.Where(i => i.TechnicianId == filterModel.TechnicianId).ToList();
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
                ds = ds.Where(i => i.ScheduleDate <= filterModel.ToDate && i.ScheduleDate >= filterModel.FromDate).ToList();
            }
            if ( filterModel.ShowAll==false &&  filterModel.AssetTagId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.TechnicianId == null&&filterModel.Cost==null)
            {
                ds = null;
            }

           
            Report = new rptAssetRepair(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
