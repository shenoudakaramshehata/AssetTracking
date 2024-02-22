using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.InsuranceManagement
{
    [Authorize]
    public class InsuranceListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
