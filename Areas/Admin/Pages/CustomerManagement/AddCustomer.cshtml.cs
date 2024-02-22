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

namespace AssetProject.Areas.Admin.Pages.CustomerManagement
{
    [Authorize]
    public class AddCustomerModel : PageModel
    {
        private readonly AssetContext _context;
        [BindProperty]
        public Customer customer { get; set; }
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AddCustomerModel(IToastNotification toastNotification, AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);
                customer.TenantId = tenant.TenantId;
                _context.Customers.Add(customer);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Customer Created Successfuly");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
            }
            return RedirectToPage("CustomerList");

        }
    }
}

