using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace AssetProject.Areas.Admin.Pages.TypeManagment
{
    [Authorize]
    public class EditTypeModel : PageModel
    {
        [BindProperty]
        public Type Type  { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;

        public EditTypeModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;
        }
        public IActionResult OnGet(int id)
        {
            Type = Context.Types.Find(id);
            if (Type == null)
            {
                return Redirect("../../Error");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (Type.BrandId == null)
            {

                ModelState.AddModelError("", "Please select Type");
                return Page();
            }
            if (ModelState.IsValid)
            {
                var UpdatedType = Context.Types.Attach(Type);
                UpdatedType.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Type Edited successfully");
                return RedirectToPage("/TypeManagment/TypeDetails", new { id = Type.TypeId });
            }
            return Page();
        }
    }
}
