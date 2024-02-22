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
    public class LeasingReportModel : PageModel
    {
        private readonly AssetContext _context;

        public LeasingReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }


        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptLeasing Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptLeasing(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            //List<LeasingModel> ds = _context.AssetLeasingDetails.Select(i => new LeasingModel
            //{
            //    AssetCost = i.Asset.AssetCost,
            //    AssetDescription = i.Asset.AssetDescription,
            //    AssetSerialNo = i.Asset.AssetSerialNo,
            //    AssetTagId = i.Asset.AssetTagId,
            //    CustomerTL = i.AssetLeasing.Customer.FullName,
            //    LeasingEndDate = i.AssetLeasing.EndDate,
            //    LeasingStartDate = i.AssetLeasing.StartDate,
            //    AssetLeasingId=i.AssetLeasing.AssetLeasingId,
            //    CustomerId=i.AssetLeasing.Customer.CustomerId,
            //    photo=i.Asset.Photo,
            //    LeasingCost=i.AssetLeasing.LeasedCost

            //}).ToList();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<LeasingModel> ds = _context.Assets.Include(i => i.AssetLeasingDetails).ThenInclude(i => i.AssetLeasing).ThenInclude(e=>e.Customer).Where(c =>c.TenantId==tenant.TenantId &&c.AssetStatusId == 6).Select(i => new LeasingModel
            {
                AssetId = i.AssetId,
                AssetCost = i.AssetCost,
                AssetSerialNo = i.AssetSerialNo,
                AssetTagId = i.AssetTagId,
                photo = i.Photo,
                LeasingEndDate = i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.EndDate,
                LeasingStartDate = i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.StartDate,
                LeasingCost = i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.LeasedCost,
                CustomerTL = _context.Customers.Where(a => a.CustomerId == i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.CustomerId).FirstOrDefault().FullName,
                CustomerId = _context.Customers.Where(a => a.CustomerId == i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.CustomerId).FirstOrDefault().CustomerId

            }).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }
            if (filterModel.AssetTagId != null)
            {
                ds = ds.Where(i => i.AssetTagId == filterModel.AssetTagId).ToList();
            }
            if (filterModel.CustomerId!=null)
            {
                ds = ds.Where(i => i.CustomerId == filterModel.CustomerId).ToList();
            }
            if (filterModel.FromDate != null && filterModel.ToDate == null)
            {
                ds = null;
            }
            if (filterModel.FromDate == null && filterModel.ToDate != null)
            {
                ds = null;
            }
            if (filterModel.FromDate!=null&&filterModel.ToDate!=null)
            {
                ds = ds.Where(i => i.LeasingStartDate <= filterModel.ToDate && i.LeasingStartDate >= filterModel.FromDate).ToList();
            }
            if (filterModel.AssetTagId == null&& filterModel.ShowAll == false && filterModel.FromDate == null && filterModel.ToDate == null&& filterModel.CustomerId == null)
            {
                ds = null;
            }
           
            Report = new rptLeasing(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
    }

