using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.VendorManagment
{
    [Authorize]
    public class VendorListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
