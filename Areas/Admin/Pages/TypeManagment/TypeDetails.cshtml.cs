using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Linq;

namespace AssetProject.Areas.Admin.Pages.TypeManagment
{
    [Authorize]
    public class TypeDetailsModel : PageModel
    {
        public Type Type { set; get; }
        AssetContext Context;
        public string BrandName;
        private readonly IToastNotification _toastNotification;
        public TypeDetailsModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;
        }
        public IActionResult OnGet(int id)
        {
            Type = Context.Types.Where(c => c.TypeId == id).Include(c => c.Brand).FirstOrDefault();
            if (Type == null)
            {
                return Redirect("../../Error");
            }
            if (Type.Brand != null)
            {
                BrandName = Type.Brand.BrandTitle;
            }
            return Page();
        }
    }
}
