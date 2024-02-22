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
    public class CategoryListModel : PageModel
    {
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        public CategoryListModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;
   
        }
        public void OnGet()
        {
        }

        public IActionResult OnPostDeleteCategory(Category cat )
        {
            Category DeletedCat = Context.Categories.FirstOrDefault(e=>e.CategoryId==cat.CategoryId);
            if (DeletedCat == null)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
            }
            Context.Categories.Remove(DeletedCat);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Category Deleted Succeffully");
               
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
           
            }
            return RedirectToPage("/SetUp/CategoryList");


        }
        public IActionResult OnGetCategoryforDelete(int categoryid ) { 
          var result=  Context.Categories.FirstOrDefault(e => e.CategoryId == categoryid);
            return new JsonResult(result);
        }
    }
}
