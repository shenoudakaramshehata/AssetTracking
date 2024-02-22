using AssetProject.Data;
using AssetProject.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.ContractManagment
{
    [Authorize]
    public class DetailsContractModel : PageModel
    {
        public Contract Contract { set; get; }
        AssetContext Context;
        public string VendorName;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DetailsContractModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
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
            if(Contract.Vendor!=null)
            {
                VendorName = Contract.Vendor.VendorTitle;
            }
            if (Contract.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }
            return Page();
        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions,int ContractId)
        {
            var assetcontracts = Context.AssetContracts.Where(e => e.ContractId == ContractId).Select(e => new
            {
                e.Asset
            });
            return new JsonResult(DataSourceLoader.Load(assetcontracts, loadOptions));
        }
    }
}
