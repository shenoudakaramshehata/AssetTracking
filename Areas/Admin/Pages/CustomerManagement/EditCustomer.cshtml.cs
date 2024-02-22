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
    public class EditCustomerModel : PageModel
    {
        private readonly AssetContext _context;
        [BindProperty]
        public Customer customer { get; set; }
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public EditCustomerModel(AssetContext context, IToastNotification toastNotification ,UserManager<ApplicationUser> userManager)
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
                    _toastNotification.AddErrorToastMessage("Something went error");
                    return RedirectToPage("CustomerList");
                }
                if (customer.TenantId != tenant.TenantId)
                {
                    return Redirect("../NotFound");
                }
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("CustomerList");
            }
            return Page();



        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Customer Update Successfuly");
                return RedirectToPage("/CustomerManagement/CustomerDetails", new { id = customer.CustomerId });

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went error");
                return RedirectToPage("/CustomerManagement/EditCustomer", new { id = customer.CustomerId });

            }
        }
    }
}

