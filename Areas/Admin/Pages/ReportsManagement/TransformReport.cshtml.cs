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
    public class TransformReportModel : PageModel
    {
        public TransformReportModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        // [BindProperty]
        //public FilterModel filterModel { get; set; }
        public ReportCheckOutForm CheckoutReport { get; set; }
        public rptCheckInForm CheckInReport { get; set; }
        public AssetContext _context { get; }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;
        public int CheckInParam { get; set; }
        public int CheckOutParam { get; set; }

        public async Task<IActionResult> OnGet(int CheckInId, int CheckOutId)
        {
            CheckInParam = CheckInId;
            CheckOutParam = CheckOutId;
            List<AssetMovement> ds = _context.AssetMovements.Include(a => a.Employee).Include(a => a.Location).Include(a => a.Store).
                Include(a => a.Department).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset).ThenInclude(a => a.Item)
                .ToList();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            tenant.Email = user.Email;
            tenant.Phone = user.PhoneNumber;
           
            CheckoutReport = new ReportCheckOutForm(tenant);
            CheckInReport = new rptCheckInForm(tenant);
            CheckInReport.DataSource = ds;

            CheckInReport.Parameters[0].Value = CheckInId;
            CheckInReport.RequestParameters = false;
            CheckoutReport.DataSource = ds;
            CheckoutReport.Parameters[0].Value = CheckOutId;
            CheckoutReport.RequestParameters = false;
            // Create the first report and generate its document.
           // XtraReport1 report1 = new XtraReport1();
            CheckInReport.CreateDocument();

            // Create the second report and generate its document.
           // XtraReport2 report2 = new XtraReport2();
            CheckoutReport.CreateDocument();

            // Add all pages of the second report to the end of the first report.
            CheckInReport.ModifyDocument(x => {
                x.AddPages(CheckoutReport.Pages);
            });

            // Preview the modified report
            return Page();

        }

    }
}

