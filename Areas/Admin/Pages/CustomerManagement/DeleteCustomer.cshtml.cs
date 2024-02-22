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
    public class DeleteCustomerModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Customer customer { get; set; }
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DeleteCustomerModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);

                customer = _context.Customers.Find(id);
                if (customer == null)
                {
                    _toastNotification.AddErrorToastMessage("Something went Error");
                    return RedirectToPage("CustomerList");
                }
                if (customer.TenantId != tenant.TenantId)
                {
                    return Redirect("../NotFound");
                }

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
                return RedirectToPage("CustomerList");
            }
            return Page();

        }
        public IActionResult OnPost(int? id)
        {
            customer = _context.Customers.Find(id);
            if (customer != null)
            {
                try
                {
                    _context.Customers.Remove(customer);
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Customer Deleted Successfully");
                    return RedirectToPage("/CustomerManagement/CustomerList");
                }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("Something went error");
                    return RedirectToPage("/CustomerManagement/DeleteCustomer", new { id = customer.CustomerId });

                }
            }
            _toastNotification.AddErrorToastMessage("Something went error");
            return RedirectToPage("/CustomerManagement/CustomerList");

        }
    }
}

