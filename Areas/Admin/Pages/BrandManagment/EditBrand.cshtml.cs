using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.BrandManagment
{
    [Authorize]
  
    public class EditBrandModel : PageModel
    {
        [BindProperty]
        public Brand Brand { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public EditBrandModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
             Context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task <IActionResult> OnGet(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);
            Brand = Context.Brands.Find(id);
            if (Brand== null)
            {
                return Redirect("../NotFound");
            }
            if (Brand.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var UpdatedBrand = Context.Brands.Attach(Brand);
                UpdatedBrand.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Brand Edited successfully");
                    return RedirectToPage("/BrandManagment/BrandDetails", new { id = Brand.BrandId });
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/BrandManagment/EditBrand", new { id = Brand.BrandId });
                }
            }
            return Page();
        }
    }
}
