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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.AssetManagment
{  
    [Authorize]
    public class SearchCheckedOutAssetsByDepartmentModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        [BindProperty]
        public int DepartmentId { set; get; }
        public static List<Asset> checkedoutassets = new List<Asset>();
        static bool isEntered = false;

        public SearchCheckedOutAssetsByDepartmentModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public void OnGet()
        {
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

        public async Task <IActionResult> OnPost()
        {
            if (DepartmentId != 0)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                tenant = _context.Tenants.Find(user.TenantId);
                checkedoutassets = new List<Asset>();
                isEntered = true;
                var movementsForDepartment = _context.AssetMovements.Where(a => a.DepartmentId == DepartmentId && a.AssetMovementDirectionId == 1 && a.EmpolyeeID == null&&a.Department.TenantId==tenant.TenantId).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
                foreach (var item in movementsForDepartment)
                {
                    foreach (var item2 in item.AssetMovementDetails)
                    {
                        if (item2.Asset.AssetStatusId == 2)
                        {
                            var lastassetmovement = _context.AssetMovementDetails.Where(a => a.AssetId == item2.AssetId && a.AssetMovement.AssetMovementDirectionId == 1).Include(a => a.AssetMovement).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                            if (lastassetmovement.AssetMovement.EmpolyeeID == null && lastassetmovement.AssetMovement.DepartmentId == DepartmentId)
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
