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
    public class PrintCategoryModel : PageModel
    {
        private readonly AssetContext _context;

        public PrintCategoryModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [BindProperty]
        public FilterModel filterModel { get; set; }
        public int AssetId { get; set; }
        public rptCategory Report { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            Report = new rptCategory(tenant);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            List<ReportModels.Category> ds = _context.Categories.Where(e=>e.TenantId==tenant.TenantId).Select(i => new ReportModels.Category
            {
                CategoryId=i.CategoryId,
               CategoryTIAR=i.CategoryTIAR

            }).ToList();
            if (filterModel.ShowAll != false)
            {
                ds = ds.ToList();
            }
            if (filterModel.CategoryId != null)
            {
                ds = ds.Where(i => i.CategoryId == filterModel.CategoryId).ToList();
            }
            if (filterModel.ShowAll == false&& filterModel.CategoryId == null)
            {
                ds = null;
            }
           
            Report = new rptCategory(tenant);
            Report.DataSource = ds;
            return Page();
        }
    }
}
