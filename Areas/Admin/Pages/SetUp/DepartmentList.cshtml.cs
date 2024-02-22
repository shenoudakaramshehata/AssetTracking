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
    public class DepartmentListModel : PageModel
    {
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        public DepartmentListModel(AssetContext context, IToastNotification toastNotification)
        {
            Context = context;
            _toastNotification = toastNotification;

        }
        public void OnGet()
        {
        }

        public IActionResult OnPostDeleteDepartment(Department dept)
        {
           var  Deleteddept = Context.Departments.FirstOrDefault(e => e.DepartmentId == dept.DepartmentId);
            if (Deleteddept == null)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
            }
            Context.Departments.Remove(Deleteddept);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Department Deleted Succeffully");

            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");

            }
            return RedirectToPage("/SetUp/DepartmentList");


        }
        public IActionResult OnGetDepartmentforDelete(int DepartmentId)
        {
            var result = Context.Departments.FirstOrDefault(e => e.DepartmentId == DepartmentId);
            return new JsonResult(result);
        }
    }
}
