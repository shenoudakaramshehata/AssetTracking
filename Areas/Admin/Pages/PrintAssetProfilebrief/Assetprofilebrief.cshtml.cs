using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetProject.Data;
using AssetProject.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.PrintAssetProfilebrief
{
    [Authorize]
    public class AssetprofilebriefModel : PageModel
    {
        private readonly AssetContext _context;

        public AssetprofilebriefModel(AssetContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int AssetId { get; set; }
        public AssetProfilebrief Report { get; set; }


        public void OnGet(int AssetId)
        {
            Report = new AssetProfilebrief();
            var ds = _context.Assets.ToList();
            Report.DataSource = ds;
            Report.Parameters[0].Value = AssetId;
            Report.RequestParameters = false;
        }
        
    }
}
