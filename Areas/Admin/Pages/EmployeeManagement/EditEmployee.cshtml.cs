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

namespace AssetProject.Areas.Admin.Pages.EmployeeManagement
{
    [Authorize]
    public class EditEmployeeModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Employee employee { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public EditEmployeeModel(AssetContext context,IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;

        }
        public async Task <IActionResult> OnGet(int? id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            
            try
                {
                    employee = _context.Employees.Find(id);

                    if (employee == null)
                    {
                        _toastNotification.AddErrorToastMessage("Something went error");
                        return RedirectToPage("EmployeeList");
                    }

                if (employee.TenantId != tenant.TenantId)
                {
                    return Redirect("../NotFound");
                }
            }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("Something went error");
                    return RedirectToPage("EmployeeList");
            }
            return Page();
            
            

        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("/EmployeeManagement/EditEmployee", new { id = employee.ID });
            try
            {
                _context.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Employee Update Successfuly");
                return RedirectToPage("/EmployeeManagement/DetailsEmployee", new { id = employee.ID });

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("/EmployeeManagement/EditEmployee", new { id = employee.ID });

            }
        }
    }
}
