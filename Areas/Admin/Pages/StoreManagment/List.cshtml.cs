using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace AssetProject.Areas.Admin.Pages.StoreManagment
{
    [Authorize]
    public class ListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
