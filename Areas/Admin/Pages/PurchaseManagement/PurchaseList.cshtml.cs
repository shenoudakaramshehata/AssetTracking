using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.PurchaseManagement
{
    [Authorize]
    public class PurchaseListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
