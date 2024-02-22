using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Collections.Generic;
using System.Linq;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AssetProject.Areas.Admin.Pages.AssetManagment
{
    [Authorize]
    public class SearchCheckedOutAssetsByEmpolyeeModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        [BindProperty]
        public int EmpolyeeID { set; get; }
        public static List<Asset> checkedoutassets = new List<Asset>();
        static bool isEntered = false;

        public SearchCheckedOutAssetsByEmpolyeeModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
            _toastNotification = toastNotification;


        }


        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions)
        {
            if (!isEntered)
            {
                checkedoutassets = new List<Asset>();

            }
            isEntered = false;
            return new JsonResult(DataSourceLoader.Load(checkedoutassets.Distinct(), loadOptions));
        }
        public void OnGet()
        {
        }

        public async Task <IActionResult> OnPost()
        {
            if (EmpolyeeID != 0)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);
                checkedoutassets = new List<Asset>();
                isEntered = true;
                var movementsForEmpolyee = _context.AssetMovements.Where(a => a.EmpolyeeID == EmpolyeeID && a.AssetMovementDirectionId == 1 &&a.Employee.TenantId==tenant.TenantId).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
                foreach (var item in movementsForEmpolyee)
                {
                    foreach (var item2 in item.AssetMovementDetails)
                    {
                        if (item2.Asset.AssetStatusId == 2)
                        {
                            var lastassetmovement = _context.AssetMovementDetails.Where(a => a.AssetId == item2.AssetId && a.AssetMovement.AssetMovementDirectionId == 1).Include(a => a.AssetMovement).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                            if (lastassetmovement.AssetMovement.EmpolyeeID == EmpolyeeID)
                            {
                                item2.Asset.AssetMovementDetails = null;
                                checkedoutassets.Add(item2.Asset);
                            }
                            
                        }
                    }
                }
            }
            return Page();
        }
    }
}
