using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Linq;

namespace AssetProject.Areas.Admin.Pages.SetUp
{
    [Authorize]
    public class LocationTreeModel : PageModel
    {
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        public LocationTreeModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;

        }
        public void OnGet()
        {
        }

        public IActionResult OnPostDeleteLocation(Location location )
        {
           var  Deletedlocation = Context.Locations.FirstOrDefault(e => e.LocationId ==location.LocationId);
            if (Deletedlocation == null)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
            }
            Context.Locations.Remove(Deletedlocation);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Location Deleted Succeffully");

            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");

            }
            return RedirectToPage("/SetUp/LocationTree");


        }
        public IActionResult OnGetLocationforDelete(int locationid)
        {
            var result = Context.Locations.FirstOrDefault(e => e.LocationId == locationid);
            return new JsonResult(result);
        }
    }
}
