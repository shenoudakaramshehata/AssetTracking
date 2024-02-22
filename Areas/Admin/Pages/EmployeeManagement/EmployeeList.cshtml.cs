using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.EmployeeManagement
{
    [Authorize]
    public class EmployeeListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
