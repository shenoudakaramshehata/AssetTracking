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

namespace AssetProject.Areas.Admin.Pages.PrintAssetProfile
{
    [Authorize]
    public class PrintAssetModel : PageModel
    {
        private readonly AssetContext _context;

        public PrintAssetModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;

        }
        [BindProperty]
        public int AssetId { get; set; }
        public rptAssetProfileList Report { get; set; }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;


        public async Task<IActionResult> OnGet(int AssetId)
        {   
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var asset = _context.Assets.Find(AssetId);
            if (asset == null)
            {
                return RedirectToPage("../NotFound");
            }
            if (asset.TenantId != tenant.TenantId)
            {
                return RedirectToPage("../NotFound");
            }
            List<Asset> ds = _context.Assets.Include(a => a.Item).Include(a => a.Store).Include(a=>a.AssetStatus).Include(e=>e.Vendor).Include(e=>e.DepreciationMethod)
                   .ToList();
           
            Report = new rptAssetProfileList(tenant);
            Report.DataSource = ds;
            Report.Parameters[0].Value = AssetId;
            Report.RequestParameters = false;
            return Page();
        }
        
    }
}
