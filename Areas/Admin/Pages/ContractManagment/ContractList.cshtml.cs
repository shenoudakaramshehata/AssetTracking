using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.ContractManagment
{
    [Authorize]
    public class ContractListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
