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
    public class AddEmployeeModel : PageModel
    {
        private readonly AssetContext _context;
        [BindProperty]
        public Employee employee { get; set; }
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;

        public AddEmployeeModel(IToastNotification toastNotification,AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;

        }
        public Tenant tenant { set; get; }

        public void OnGet()
        {

        }
        public async Task <IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
                 return Page();
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);
                employee.TenantId = tenant.TenantId;
                _context.Employees.Add(employee);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Employee Created Successfuly");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
            }
            return RedirectToPage("EmployeeList");
            
        }
    }
}
