using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.PatchProcess
{
    [Authorize]
    public class PatchCheckInModel : PageModel
    {
        [BindProperty]
        public AssetMovement assetmovement { set; get; }
        AssetContext _context;
        public static List<Asset> SelectedAssets = new List<Asset>();
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public PatchCheckInModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            assetmovement = new AssetMovement();
            UserManger = userManager;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPostFillAssetList([FromBody] List<Asset> assets)
        {

            SelectedAssets = assets;
            return new JsonResult(assets);
        }

        public IActionResult OnGetSingleAssetForView(int AssetId)
        {
            var Result = _context.Assets.Where(c => c.AssetId == AssetId).Include(a => a.Item).Include(a => a.DepreciationMethod).FirstOrDefault();
            return new JsonResult(Result);
        }
        public async Task <IActionResult> OnGetGridData(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var Assets = _context.Assets.Where(a => a.AssetStatusId==2&&a.TenantId==tenant.TenantId).Select(i => new {
                i.AssetId,
                i.AssetDescription,
                i.AssetTagId,
                i.AssetCost,
                i.AssetSerialNo,
                i.AssetPurchaseDate,
                i.ItemId,
                i.Photo,
                i.DepreciableAsset,
                i.DepreciableCost,
                i.SalvageValue,
                i.AssetLife,
                i.DateAcquired,
                i.Item.ItemTitle,
                i.DepreciationMethodId,
                i.VendorId,
                i.StoreId,
                i.AssetStatusId,
                i.TenantId,
                i.PurchaseNo
            });

            return new JsonResult(DataSourceLoader.Load(Assets, loadOptions));
        }

        public IActionResult OnPost()
        {
            if (assetmovement.StoreId == null)
            {
                ModelState.AddModelError("", "Please Select Store");
                SelectedAssets = null;
                return Page();
            }
            //if (assetmovement.ActionTypeId == null)
            //{
            //    ModelState.AddModelError("", "Please Select Action");
            //    SelectedAssets = null;

            //    return Page();
            //}
            //if (assetmovement.LocationId == null)
            //{
            //    ModelState.AddModelError("", "Please Select Location");
            //    SelectedAssets = null;

            //    return Page();
            //}
            //if (assetmovement.DepartmentId == null)
            //{
            //    ModelState.AddModelError("", "Please Select Department");
            //    SelectedAssets = null;

            //    return Page();
            //}
            //if (assetmovement.ActionTypeId == 1)
            //{
            //    if (assetmovement.EmpolyeeID == null)
            //    {
            //        ModelState.AddModelError("", "Please Select Empolyee");
            //        SelectedAssets = null;

            //        return Page();
            //    }
            //}

            if (ModelState.IsValid)
            {
                if (SelectedAssets != null)
                {
                if (SelectedAssets.Count != 0)
                {

                    assetmovement.AssetMovementDirectionId = 2;
                        assetmovement.TransactionDate = DateTime.Now;
                    assetmovement.AssetMovementDetails = new List<AssetMovementDetails>();
                    string DirectionTitle = "Direction Title : ";
                    string TransDate = "Transaction Date : ";
                    AssetMovementDirection Direction = _context.AssetMovementDirections.Find(assetmovement.AssetMovementDirectionId);
                    string TransactionDate = assetmovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    foreach (var asset in SelectedAssets)
                    {
                        asset.AssetStatusId = 1;
                        var UpdatedAsset = _context.Assets.Attach(asset);
                        UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        assetmovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });
                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 16,
                            AssetId =asset.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"{TransDate}{TransactionDate} and {DirectionTitle}{Direction.AssetMovementDirectionTitle}")
                        };
                        _context.AssetLogs.Add(assetLog);
                    }
                    _context.AssetMovements.Add(assetmovement);
                    try
                    {
                        _context.SaveChanges();
                            SelectedAssets = null;

                        }
                        catch (Exception e)
                    {
                        _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                            SelectedAssets = null;
                        return Page();
                    }
                    _toastNotification.AddSuccessToastMessage("Asset Movements Added successfully");
                        SelectedAssets = null;

                        return RedirectToPage("/ReportsManagement/CheckInFormRPT", new { AssetMovement = assetmovement.AssetMovementId });
                }
                }

                _toastNotification.AddErrorToastMessage("Please Select at Least one Asset");
                return Page();
            }
            _toastNotification.AddErrorToastMessage("Something went Error,Try again");
            SelectedAssets = null;
            return Page();
        }

    }
}
