using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.AssetManagment
{
    [Authorize]
    public class GetAssetRepairModel : PageModel
    {
   
        public void OnGet()
        {
        }
    }
}
