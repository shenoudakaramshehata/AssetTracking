using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.AssetManagment
{
    [Authorize]
    public class SearchAssetModel : PageModel
    {
        
        private readonly AssetContext _context ;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        [BindProperty]
        public AssetSerachVM AssetSerachVM { get; set; }
        public SearchAssetModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public void OnGet()
        {
        }
        public async Task <IActionResult> OnPost()
        {
            bool CheckSearchItem=false;
            int SearchItem = 0;
            CheckSearchItem = int.TryParse(AssetSerachVM.AssetSearchItem, out SearchItem);
            
            if (ModelState.IsValid)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);
                List<Asset> ListOfAssets = _context.Assets
                    .Where(x =>x.TenantId == tenant.TenantId &&(x.AssetTagId == AssetSerachVM.AssetSearchItem || x.AssetSerialNo == AssetSerachVM.AssetSearchItem || x.AssetCost == SearchItem||x.AssetLife== SearchItem||x.Item.ItemTitle ==AssetSerachVM.AssetSearchItem)).ToList();
                if (ListOfAssets.Count == 0)
                {
                    _toastNotification.AddErrorToastMessage("This Asset Not Found");
                    return Page();
                }
                return RedirectToPage("/AssetManagment/AssetProfile",new { AssetId = ListOfAssets[0].AssetId});
            }
            return Page();
        }
    }
}
