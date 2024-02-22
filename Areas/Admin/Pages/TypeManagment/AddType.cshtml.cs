using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace AssetProject.Areas.Admin.Pages.TypeManagment
{
    [Authorize]
    public class AddTypeModel : PageModel
    {
        [BindProperty]
        public Type Type { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        public AddTypeModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;
          Type= new Type();
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (Type.BrandId == null)
            {

                ModelState.AddModelError("", "Please Select Brand");
                return Page();
            }
            if (ModelState.IsValid)
            {
                Context.Types.Add(Type);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Type Added successfully");
                return RedirectToPage("/TypeManagment/TypeList");
            }
            return Page();
        }
    }
}
