using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetProject.Areas.Admin.Pages.SetUp
{
    [Authorize]
    public class SubCategoryListModel : PageModel
    {
       
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        public List<SubCategory> subCategories;
        public int catId { set; get; }
        public SubCategoryListModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;
           
        }
        public void OnGet(int id)
        {
            catId = id; 
        }

        public IActionResult OnPostDeleteSubCategory(SubCategory subcat)
        {
            SubCategory DeletedsubCat = Context.SubCategories.FirstOrDefault(e => e.SubCategoryId == subcat.SubCategoryId);
            if (DeletedsubCat == null)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
            }
            Context.SubCategories.Remove(DeletedsubCat);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("SubCategory Deleted Succeffully");

            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");

            }
            return RedirectToPage("/SetUp/SubCategoryList",new { id= DeletedsubCat.CategoryId });


        }
        public IActionResult OnGetSubCategoryforDelete(int Subcategoryid)
        {
            var result = Context.SubCategories.FirstOrDefault(e => e.SubCategoryId == Subcategoryid);
            return new JsonResult(result);
        }
    }
}
