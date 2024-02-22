using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace AssetProject.Areas.Admin.Pages.AssetLeasingManagement
{
    [Authorize]
    public class AddAssetLeasingModel : PageModel
    {
        [BindProperty]
        public AssetLeasing AssetLeasing { get; set; }
        public AssetContext Context { get; }
        public IToastNotification ToastNotification { get; }
        public AddAssetLeasingModel(AssetContext Context, IToastNotification toastNotification)
        {
            this.Context = Context;
            ToastNotification = toastNotification;
        }


        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Context.AssetLeasings.Add(AssetLeasing);
                Context.SaveChanges();
                ToastNotification.AddSuccessToastMessage("Asset Leasing Added successfully");
                return RedirectToPage("/AssetLeasingManagement/AsselLeasingsList");
            }
            return Page();
        }
    }
}
