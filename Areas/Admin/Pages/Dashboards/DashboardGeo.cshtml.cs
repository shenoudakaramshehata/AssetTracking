using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.Dashboards
{
    [Authorize]
    public class DashboardGeoModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
