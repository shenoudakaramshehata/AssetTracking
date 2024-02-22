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
    public class PatchTansferModel : PageModel
    {
        [BindProperty]
        public AssetMovement assetmovement { set; get; }
        AssetContext _context;
        public static List<Asset> SelectedFromAssets = new List<Asset>();
        public static List<Asset> SelectedToAssets = new List<Asset>();

        public List<Asset> EmpoyeeAssets = new List<Asset>();
        public List<Asset> DepartmentAssets = new List<Asset>();

        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        [BindProperty]
        public int? FromDepartmentId { get; set; }

        [BindProperty]
        public int? FromEmployeeId { get; set; }

        [BindProperty]
        public int? ToEmployeeId { get; set; }
        [BindProperty]
        public int? ToDepartmentId { get; set; }

        [BindProperty]
        public int? ToActionTypeId { get; set; }
        [BindProperty]
        public int? FromActionTypeId { get; set; }

        [BindProperty]
        public int? ToLocationId { get; set; }
        [BindProperty]
        public int? FromLocationId { get; set; }
        [BindProperty]
        public int? FromStoreId { get; set; }

        public PatchTansferModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            assetmovement = new AssetMovement();
            UserManger = userManager;
        }

        public void OnGet()
        {

        }
        public IActionResult OnGetFromGridData(DataSourceLoadOptions loadOptions)
        {

            return new JsonResult(DataSourceLoader.Load(DepartmentAssets, loadOptions));
        }
        public IActionResult OnGetToGridData(DataSourceLoadOptions loadOptions)
        {

            return new JsonResult(DataSourceLoader.Load(DepartmentAssets, loadOptions));
        }

        public IActionResult OnPostFillFromAssetList([FromBody] List<Asset> assets)
        {

            SelectedFromAssets = assets;
            return new JsonResult(assets);
        }
        public IActionResult OnPostTransferFrom([FromBody] List<Asset> assets)
        {

            if (FromActionTypeId == null)
            {
                ModelState.AddModelError("FromActionTypeError", "Please Select Action");
                return Page();
            }
            else if (FromActionTypeId == 1)
            {
                if (FromDepartmentId == null)
                {
                    ModelState.AddModelError("FromDepartmentError", "Please Select Department");
                    return Page();
                }
                if (FromEmployeeId == null)
                {
                    ModelState.AddModelError("FromEmployeeError", "Please Select Empolyee");
                    return Page();
                }

            }
            else if (FromActionTypeId == 2)
            {
                if (FromDepartmentId == null)
                {
                    ModelState.AddModelError("FromDepartmentError", "Please Select Department");
                    return Page();
                }

            }
            if (FromLocationId == null)
            {
                ModelState.AddModelError("FromLocationError", "Please Select Location");
                return Page();
            }

            if (FromStoreId == null)
            {
                ModelState.AddModelError("FromStoreError", "Please Select Store");
                return Page();
            }

            if (ToActionTypeId == null)
            {
                ModelState.AddModelError("ToActionTypeError", "Please Select Action");
                return Page();
            }
            else if (ToActionTypeId == 1)
            {
                if (ToDepartmentId == null)
                {
                    ModelState.AddModelError("ToDepartmentError", "Please Select Department");
                    return Page();
                }
                if (ToEmployeeId == null)
            {
                ModelState.AddModelError("ToEmployeeError", "Please Select Empolyee");
                return Page();
            }

            }
            else if (ToActionTypeId == 2)
            {
                if (ToDepartmentId == null)
            {
                ModelState.AddModelError("ToDepartmentError", "Please Select Department");
                return Page();
            }


            }
           
            if ( ToLocationId == null)
            {
                ModelState.AddModelError("ToLocationError", "Please Select Location");
                return Page();
            }
           


            //Inert two movement

            //First move assets to store --> Check in


            if (SelectedFromAssets.Count != 0)
            {
                int CheckInID = checkin(SelectedFromAssets);
                if (CheckInID == 0)
                {
                    _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                    return Page();
                }

                //Second move asset from store to department
                int CheckoutID = checkout( SelectedFromAssets);
                if (CheckoutID == 0)
                {
                    _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                    return Page();
                }

                //Print check in form
                _toastNotification.AddSuccessToastMessage("Asset Movements Added successfully");
                return RedirectToPage("/ReportsManagement/MergeTwoReport", new { CheckInId = CheckInID, CheckOutId = CheckoutID });
                //Print check out form

            }

            _toastNotification.AddErrorToastMessage("Please Select at Least one Asset");
            return Page();


        }

        public int checkin( List<Asset> selectedAssetsList)
        {
            AssetMovement newAssetMovement = null;
            if (selectedAssetsList.Count != 0)
            {
                if (FromActionTypeId == 1)
                {
                    newAssetMovement = new AssetMovement()
                    {
                        AssetMovementDirectionId = 2,
                        EmpolyeeID=FromEmployeeId,

                        ActionTypeId = 1,
                        DepartmentId = FromDepartmentId,
                        LocationId = FromLocationId,
                        TransactionDate = DateTime.Now,
                        //DueDate=assetMovementObj.DueDate,
                        //Remarks = assetMovementObj.Remarks,
                        StoreId = FromStoreId,

                    };

                }
                else if (FromActionTypeId == 2)
                {
                    newAssetMovement = new AssetMovement()
                    {
                        AssetMovementDirectionId = 2,
                        DepartmentId = FromDepartmentId,
                        ActionTypeId = 2,
                        //DepartmentId=assetMovementObj.DepartmentId,
                        LocationId = FromLocationId,
                        TransactionDate = DateTime.Now,
                        //DueDate=assetMovementObj.DueDate,
                        //Remarks = assetMovementObj.Remarks,
                        StoreId = FromStoreId,
                    };
                    }
                
                newAssetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                string DirectionTitle = "Direction Title : ";
                string TransDate = "Transaction Date : ";
                AssetMovementDirection Direction = _context.AssetMovementDirections.Find(newAssetMovement.AssetMovementDirectionId);
                //assetMovementObj.TransactionDate = DateTime.Now;
                string TransactionDate = newAssetMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                foreach (var asset in selectedAssetsList)
                {
                    asset.AssetStatusId = 1;
                    var UpdatedAsset = _context.Assets.Attach(asset);
                    UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    newAssetMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });
                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 16,
                        AssetId = asset.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($"{TransDate}{TransactionDate} and {DirectionTitle}{Direction.AssetMovementDirectionTitle} Transfered")
                    };
                    _context.AssetLogs.Add(assetLog);
                }

                _context.AssetMovements.Add(newAssetMovement);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
            return newAssetMovement.AssetMovementId;
        }

        public int checkout( List<Asset> selectedAssetsList)
        {
            AssetMovement newAssetMovement = null;
            if (selectedAssetsList.Count != 0)
            {
                if (ToActionTypeId == 1)
                {
                    newAssetMovement = new AssetMovement()
                    {
                        AssetMovementDirectionId = 1,
                        ActionTypeId = 1,
                        DepartmentId = ToDepartmentId,
                        LocationId = ToLocationId,
                        //StoreId = assetMovementObj.StoreId,
                        //Remarks = assetMovementObj.Remarks,
                        TransactionDate = DateTime.Now,
                        EmpolyeeID = ToEmployeeId,
                        //DueDate = assetMovementObj.DueDate
                    };

                }
                else if(ToActionTypeId == 2)
                {
                    newAssetMovement = new AssetMovement()
                    {
                        AssetMovementDirectionId = 1,
                        ActionTypeId = 2,
                        DepartmentId = ToDepartmentId,
                        LocationId = ToLocationId,
                        //StoreId = assetMovementObj.StoreId,
                        //Remarks = assetMovementObj.Remarks,
                        TransactionDate = DateTime.Now,
                        //EmpolyeeID = assetMovementObj.EmpolyeeID,
                        //DueDate = assetMovementObj.DueDate
                    };
                }

                newAssetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                string ActionTitle = "Action Title : ";
                string TransDate = "Transaction Date : ";
                string DirectionTitle = "Direction Title : ";
                ActionType SelectedActionType = _context.ActionTypes.Find(newAssetMovement.ActionTypeId);
                AssetMovementDirection Direction = _context.AssetMovementDirections.Find(newAssetMovement.AssetMovementDirectionId);
                string TransactionDate = newAssetMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                foreach (var asset in selectedAssetsList)
                {
                    asset.AssetStatusId = 2;
                    var UpdatedAsset = _context.Assets.Attach(asset);
                    UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    newAssetMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });

                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 17,
                        AssetId = asset.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($"{TransDate}{TransactionDate} and {ActionTitle}{SelectedActionType.ActionTypeTitle} and {DirectionTitle}{Direction.AssetMovementDirectionTitle} Transfered")
                    };
                    _context.AssetLogs.Add(assetLog);
                }
                _context.AssetMovements.Add(newAssetMovement);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
            return newAssetMovement.AssetMovementId;
        }


        public IActionResult OnPostFillToAssetList([FromBody] List<Asset> assets)
        {

            SelectedToAssets = assets;
            return new JsonResult(assets);
        }

      
        //public IActionResult OnPostTransferTo([FromBody] List<Asset> assets)
        //{

        //    SelectedToAssets = assets;
        //    return new JsonResult(assets);
        //}
        
        public IActionResult OnGetAssetsForDepartment(string values)
        {
            var DepartmentId = JsonConvert.DeserializeObject<int>(values);
            var movementsForDepartment = _context.AssetMovements.Where(a => a.DepartmentId == DepartmentId && a.AssetMovementDirectionId == 1 && a.EmpolyeeID == null).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
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
                            DepartmentAssets.Add(item2.Asset);
                        }

                    }
                }
            }

            return new JsonResult(DepartmentAssets.Distinct());
        }


        public IActionResult OnGetAssetsForEmpolyee(string values)
        {
            var EmpoyeeId = JsonConvert.DeserializeObject<int>(values);
            var movementsForEmpolyee = _context.AssetMovements.Where(a => a.EmpolyeeID == EmpoyeeId && a.AssetMovementDirectionId == 1).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
            foreach (var item in movementsForEmpolyee)
            {
                foreach (var item2 in item.AssetMovementDetails)
                {
                    if (item2.Asset.AssetStatusId == 2)
                    {
                        var lastassetmovement = _context.AssetMovementDetails.Where(a => a.AssetId == item2.AssetId && a.AssetMovement.AssetMovementDirectionId == 1).Include(a => a.AssetMovement).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                        if (lastassetmovement.AssetMovement.EmpolyeeID == EmpoyeeId)
                        {
                            item2.Asset.AssetMovementDetails = null;
                            EmpoyeeAssets.Add(item2.Asset);
                        }
                    }
                }
            }

            return new JsonResult(EmpoyeeAssets.Distinct());
        }

    }
}
