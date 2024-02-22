using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetProject.Areas.Admin.Pages.AssetLeasingManagement
{
    [Authorize]
    public class LeasingListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
