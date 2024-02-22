using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.AssetManagment
{
    [Authorize]
    public class AssetListModel : PageModel
    {
        public void OnGet()
        {
        }

       
        public void OnPostGrid()
        {

        }
    }
}
