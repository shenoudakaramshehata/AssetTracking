using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ReportModels;
using AssetProject.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    public class ExprtRFIDModel : PageModel
    {
            public ExprtRFIDModel()
            {
            }

      
            public rptRfidasset Report { get; set; }

            public IActionResult OnGet(string tagId,string serialno)
            {
            
                Report = new rptRfidasset(tagId, serialno);
                return Page();
            }
        }
}
