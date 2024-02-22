using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ReportModels;
using AssetProject.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    [Authorize]
    public class DepartmentRPTModel : PageModel
    {
        private readonly AssetContext _context;

        public DepartmentRPTModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        [BindProperty]
        public FilterModel filterModel { get; set; }
        public rptDepartmentReport Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptDepartmentReport(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<Department> ds = _context.Departments.Where(e=>e.TenantId==tenant.TenantId).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }
            if (filterModel.DepartmentId != null)
            {
                ds = ds.Where(i => i.DepartmentId == filterModel.DepartmentId).ToList();
            }
            if (filterModel.ShowAll == false&& filterModel.DepartmentId == null)
            {
                ds = new List<Department>();
            }
           
            Report = new rptDepartmentReport(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
