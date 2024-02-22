using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.ContractManagment
{
    [Authorize]
    public class DeleteContractModel : PageModel
    {
     
        public Contract Contract { set; get; }
        AssetContext Context;
        public string VendorName;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DeleteContractModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);

            Contract = Context.Contracts.Where(c => c.ContractId == id).Include(c => c.Vendor).FirstOrDefault();
            if(Contract==null)
            {
                return Redirect("../NotFound");
            }
            if (Contract.Vendor != null)
            {
                VendorName = Contract.Vendor.VendorTitle;
            }
            if (Contract.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            Contract = Context.Contracts.Find(id);
            if(Contract != null)
            {
            
                    Context.Contracts.Remove(Contract);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Contract Deleted successfully");
                    return RedirectToPage("/ContractManagment/ContractList");
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/ContractManagment/DeleteContract", new { id = Contract.ContractId });
                }
            }

            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("/ContractManagment/ContractList");

        }
    }
}
