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

namespace AssetProject.Areas.Admin.Pages.ContractManagment
{
    [Authorize]
    public class AddContractModel : PageModel
    {
        [BindProperty]
        public Contract Contract { set; get; }
        AssetContext Context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AddContractModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
           _toastNotification = toastNotification;
            Contract = new Contract();
            UserManger = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (Contract.VendorId == null)
            {
                ModelState.AddModelError("", "Please Select Vendor");
                return Page();
            }
            if (Contract.EndDate <= Contract.StartDate)
            {
                ModelState.AddModelError("", "EndDate mustbe greater than StartDate  ");
                return Page();
            }
            if (ModelState.IsValid)
            {

                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = Context.Tenants.Find(user.TenantId);
                Contract.TenantId = tenant.TenantId;
                Context.Contracts.Add(Contract);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Contract Added successfully");
                    return RedirectToPage("/ContractManagment/ContractList");
                }
               catch(Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/ContractManagment/ContractList");
                }
            }
            return Page();
        }
    }
}
