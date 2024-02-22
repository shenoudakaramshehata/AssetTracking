using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.TypeManagment
{
    [Authorize]
    public class TypeListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
