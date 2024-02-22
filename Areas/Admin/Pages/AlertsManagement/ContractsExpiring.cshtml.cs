using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.Alerts_Management
{
    [Authorize]

    public class ContractsExpiringModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
