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
    public class ContractReportModel : PageModel
    {
        private readonly AssetContext _context;

        public ContractReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptContract Report { get; set; }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;
        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptContract(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<ContractModel> ds = _context.AssetContracts.Include(e=>e.Asset).Where(e=>e.Asset.TenantId==tenant.TenantId).Select(i => new ContractModel
            {
                AssetCost = i.Asset.AssetCost,
                AssetDescription = i.Asset.AssetDescription,
                AssetSerialNo = i.Asset.AssetSerialNo,
                AssetTagId = i.Asset.AssetTagId,
                ContractNo = i.Contract.ContractNo,
                ContractTL = i.Contract.Title,
                ContractEndDate = i.Contract.StartDate,
                ItemTL = i.Asset.Item.ItemTitle,
                ContractStartDate = i.Contract.StartDate,
                Cost = i.Contract.Cost,
                Photo = i.Asset.Photo,
                ContractId=i.ContractId
            }).ToList();
            if (filterModel.ContractId != null)
            {
                ds = ds.Where(i => i.ContractId == filterModel.ContractId).ToList();
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId == filterModel.AssetTagId).ToList();
            }
            if (filterModel.AssetTagId == null && filterModel.ContractId == null)
            {
                ds = new List<ContractModel>();
            }
            Report = new rptContract(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
