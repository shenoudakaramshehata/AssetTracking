using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.BrandManagment
{
    [Authorize]
    public class BrandListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
