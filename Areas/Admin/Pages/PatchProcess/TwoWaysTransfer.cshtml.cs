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
    public class TwoWaysTransferModel : PageModel
    {
        [BindProperty]
        public AssetMovement assetmovement { set; get; }
        AssetContext _context;
        public static List<Asset> SelectedLeftAssets= new List<Asset>();
        public static List<Asset> SelectedRightAssets= new List<Asset>();

     

        public static List<Asset> LeftDataSource=new List<Asset>();
        public static List<Asset> RightDataSource=new List<Asset>();

        public static List<Asset> RightDepartmentDataSource = new List<Asset>();
        public static List<Asset> RightEmployeeDataSource = new List<Asset>();

        public static List<Asset> LeftDepartmentDataSource = new List<Asset>();
        public static List<Asset> LeftEmployeeDataSource = new List<Asset>();


        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        [BindProperty]
        public int? LeftDepartmentId { get; set; }
        [BindProperty]
        public int? LeftLocationId { get; set; }
        [BindProperty]
        public int? LeftStoreId { get; set; }

        [BindProperty]
        public int? LeftEmployeeId { get; set; }
        [BindProperty]
        public int? LeftActionTypeId { get; set; }

        [BindProperty]
        public int? RightEmployeeId { get; set; }
        [BindProperty]
        public int? RightDepartmentId { get; set; }
        [BindProperty]
        public int? RightStoreId { get; set; }
        [BindProperty]
        public int? RightActionTypeId { get; set; }

        [BindProperty]
        public int? RightLocationId { get; set; }

        public TwoWaysTransferModel(AssetContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            assetmovement = new AssetMovement();
            UserManger = userManager;
        }

        public void OnGet()
        {

        }
        public IActionResult OnGetLeftGridData(DataSourceLoadOptions loadOptions)
        {

            foreach (var item in LeftDataSource)
            {
                item.AssetMovementDetails = null;
            }
            return new JsonResult(DataSourceLoader.Load(LeftDataSource, loadOptions));
        }
        public IActionResult OnGetRightGridData(DataSourceLoadOptions loadOptions)
        {
            foreach (var item in RightDataSource)
            {
                item.AssetMovementDetails = null;
            }

            return new JsonResult(DataSourceLoader.Load(RightDataSource, loadOptions));
        }

        public IActionResult OnPostFillRightAssetList([FromBody] List<Asset> assets)
        {

            SelectedRightAssets = assets;
            return new JsonResult(assets);
        }
        public IActionResult OnPostFillLeftAssetList([FromBody] List<Asset> assets)
        {

            SelectedLeftAssets = assets;
            return new JsonResult(assets);
        }


        public IActionResult OnGetTransferFromLeftToRight()
        {
            if (RightDataSource != null && SelectedLeftAssets != null)
            {
                foreach (var item in SelectedLeftAssets)
                {
                    RightDataSource.Add(item);
                    var deteleditem = LeftDataSource.FirstOrDefault(e => e.AssetId == item.AssetId);
                    var deleted = LeftDataSource.Remove(deteleditem);
                }

            }
            return new JsonResult(RightDataSource);

        }

        public IActionResult OnGetRightDepartmentAssets(string values)
        {
            RightDepartmentDataSource = new List<Asset>();
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
                            RightDepartmentDataSource.Add(item2.Asset);
                        }

                    }
                }
            }
            RightDataSource = new List<Asset>();
            RightDataSource = RightDepartmentDataSource.Distinct().ToList();
            return new JsonResult(RightDepartmentDataSource.Distinct());
        }


        public IActionResult OnGetRightEmpolyeeAssets(string values)
        {
            RightEmployeeDataSource = new List<Asset>();
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
                            RightEmployeeDataSource.Add(item2.Asset);
                        }
                    }
                }
            }
            RightDataSource = new List<Asset>();
            RightDataSource = RightEmployeeDataSource.Distinct().ToList();
            return new JsonResult(RightEmployeeDataSource.Distinct());
        }


        public IActionResult OnGetTransferFromRightToLeft()
        {
            if (LeftDataSource != null && SelectedRightAssets != null)
            {
                foreach (var item in SelectedRightAssets)
                {
                    LeftDataSource.Add(item);
                    var deteleditem = RightDataSource.FirstOrDefault(e => e.AssetId == item.AssetId);
                    var deleted= RightDataSource.Remove(deteleditem);
                }

            }
            return new JsonResult(LeftDataSource);

        }


        public IActionResult OnGetLeftDepartmentAssets(string values)
        {
            LeftDepartmentDataSource = new List<Asset>();
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
                            LeftDepartmentDataSource.Add(item2.Asset);
                        }

                    }
                }
            }
            LeftDataSource = new List<Asset>();
            LeftDataSource = LeftDepartmentDataSource.Distinct().ToList();
            return new JsonResult(LeftDepartmentDataSource.Distinct());
        }

        public IActionResult OnGetLeftEmpolyeeAssets(string values)
        {
            LeftEmployeeDataSource = new List<Asset>();
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
                            LeftEmployeeDataSource.Add(item2.Asset);
                        }
                    }
                }
            }
            LeftDataSource = new List<Asset>();
            LeftDataSource = LeftEmployeeDataSource.Distinct().ToList();
            return new JsonResult(LeftEmployeeDataSource.Distinct());
        }


        public IActionResult OnPostTransferFrom()
        {

            if (LeftActionTypeId == null)
            {
                ModelState.AddModelError("LeftActionTypeError", "Please Select Action");
                return Page();
            }
            else if (LeftActionTypeId == 1)
            {
                if (LeftDepartmentId == null)
                {
                    ModelState.AddModelError("LeftDepartmentError", "Please Select Department");
                    return Page();
                }
                if (LeftEmployeeId == null)
                {
                    ModelState.AddModelError("LeftEmployeeError", "Please Select Empolyee");
                    return Page();
                }

            }
            else if (LeftActionTypeId == 2)
            {
                if (LeftDepartmentId == null)
                {
                    ModelState.AddModelError("LeftDepartmentError", "Please Select Department");
                    return Page();
                }

            }
            if (LeftLocationId == null)
            {
                ModelState.AddModelError("LeftLocationError", "Please Select Location");
                return Page();
            }

            if (LeftStoreId == null)
            {
                ModelState.AddModelError("LeftStoreError", "Please Select Store");
                return Page();
            }

            if (RightActionTypeId == null)
            {
                ModelState.AddModelError("RightActionTypeError", "Please Select Action");
                return Page();
            }
            else if (RightActionTypeId == 1)
            {
                if (RightDepartmentId == null)
                {
                    ModelState.AddModelError("RightDepartmentError", "Please Select Department");
                    return Page();
                }
                if (RightEmployeeId == null)
                {
                    ModelState.AddModelError("RightEmployeeError", "Please Select Empolyee");
                    return Page();
                }

            }
            else if (RightActionTypeId == 2)
            {
                if (RightDepartmentId == null)
                {
                    ModelState.AddModelError("RightDepartmentError", "Please Select Department");
                    return Page();
                }


            }

            if (RightLocationId == null)
            {
                ModelState.AddModelError("RightLocationError", "Please Select Location");
                return Page();
            }

            if (RightStoreId == null)
            {
                ModelState.AddModelError("RightStoreError", "Please Select Store");
                return Page();
            }

            //Insert two movement
            if (SelectedLeftAssets.Count == 0 && SelectedRightAssets.Count == 0) {

                _toastNotification.AddErrorToastMessage("Please!Select at least one Asset from anySide to transfer ");
                return Page();
            }
            if (SelectedLeftAssets.Count != 0)
            {
                bool exist = false;
                foreach (var item in SelectedLeftAssets)
                {
                    exist = RightDataSource.Any(e => e.AssetId == item.AssetId);
                    if (!exist)
                    {
                        break;
                    }
                }
                if (exist)
                {
                    if (RightActionTypeId == 1)
                    {
                        
                        foreach (var item in RightEmployeeDataSource.Distinct())
                        {
                            if (SelectedLeftAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = SelectedLeftAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                SelectedLeftAssets.Remove(removeitem);
                            }
                        }
                    }
                    if (RightActionTypeId == 2)
                    {
                        foreach (var item in RightDepartmentDataSource.Distinct())
                        {
                            if (SelectedLeftAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = SelectedLeftAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                SelectedLeftAssets.Remove(removeitem);
                            }
                        }
                    }
                    //checkIn Left Assets To Store 
                    AssetMovement LeftCheckInMovement = null;
                    
                        if (LeftActionTypeId == 1)
                        {
                        LeftCheckInMovement = new AssetMovement()
                            {
                                AssetMovementDirectionId = 2,
                                EmpolyeeID = LeftEmployeeId,
                                ActionTypeId = 1,
                                DepartmentId = LeftDepartmentId,
                                LocationId = LeftLocationId,
                                TransactionDate = DateTime.Now,
                                StoreId = LeftStoreId,
                            };
                        }
                        else if (LeftActionTypeId == 2)
                        {
                        LeftCheckInMovement = new AssetMovement()
                            {
                                AssetMovementDirectionId = 2,
                                DepartmentId = LeftDepartmentId,
                                ActionTypeId = 2,
                                LocationId = LeftLocationId,
                                TransactionDate = DateTime.Now,
                                StoreId = LeftStoreId,
                            };
                        }

                    LeftCheckInMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                        string LeftCheckInDirectionTitle = "Direction Title : ";
                        string LeftCheckInTransDate = "Transaction Date : ";
                        AssetMovementDirection LeftCheckInDirection = _context.AssetMovementDirections.Find(LeftCheckInMovement.AssetMovementDirectionId);
                        string TransactionDate = LeftCheckInMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                        foreach (var asset in SelectedLeftAssets)
                        {
                            asset.AssetStatusId = 1;
                            var UpdatedAsset = _context.Assets.Attach(asset);
                            UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        LeftCheckInMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });
                            AssetLog assetLog = new AssetLog()
                            {
                                ActionLogId = 16,
                                AssetId = asset.AssetId,
                                ActionDate = DateTime.Now,
                                Remark = string.Format($"{LeftCheckInTransDate}{TransactionDate} and {LeftCheckInDirectionTitle}{LeftCheckInDirection.AssetMovementDirectionTitle} Transfered")
                            };
                            _context.AssetLogs.Add(assetLog);
                        }

                        _context.AssetMovements.Add(LeftCheckInMovement);
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                        _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                        return Page();
                    }
                    //checkOut Left Assets To Store 

                    AssetMovement LeftCheckOutMovement = null;
                   
                        if (RightActionTypeId == 1)
                        {
                        LeftCheckOutMovement = new AssetMovement()
                            {
                                AssetMovementDirectionId = 1,
                                ActionTypeId = 1,
                                DepartmentId = RightDepartmentId,
                                LocationId = RightLocationId,
                                StoreId = LeftStoreId,
                                TransactionDate = DateTime.Now,
                                EmpolyeeID = RightEmployeeId,
                            };

                        }
                        else if (RightActionTypeId == 2)
                        {
                        LeftCheckOutMovement = new AssetMovement()
                            {
                                AssetMovementDirectionId = 1,
                                ActionTypeId = 2,
                                DepartmentId = RightDepartmentId,
                                LocationId = RightLocationId,
                                StoreId = LeftStoreId,
                                TransactionDate = DateTime.Now,
                            };
                        }

                    LeftCheckOutMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                        string ActionTitle = "Action Title : ";
                        string LeftCheckoutTransDate = "Transaction Date : ";
                        string LeftCheckOutDirectionTitle = "Direction Title : ";
                        ActionType SelectedActionType = _context.ActionTypes.Find(LeftCheckOutMovement.ActionTypeId);
                        AssetMovementDirection LeftCheckOutDirection = _context.AssetMovementDirections.Find(LeftCheckOutMovement.AssetMovementDirectionId);
                        string CheckoutTransactionDate = LeftCheckOutMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                        foreach (var asset in SelectedLeftAssets)
                        {
                            asset.AssetStatusId = 2;
                            var UpdatedAsset = _context.Assets.Attach(asset);
                            UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        LeftCheckOutMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });

                            AssetLog assetLog = new AssetLog()
                            {
                                ActionLogId = 17,
                                AssetId = asset.AssetId,
                                ActionDate = DateTime.Now,
                                Remark = string.Format($"{LeftCheckoutTransDate}{TransactionDate} and {ActionTitle}{SelectedActionType.ActionTypeTitle} and {LeftCheckOutDirectionTitle}{LeftCheckOutDirection.AssetMovementDirectionTitle} Transfered")
                            };
                            _context.AssetLogs.Add(assetLog);
                        }
                        _context.AssetMovements.Add(LeftCheckOutMovement);
                        try
                        {
                            _context.SaveChanges();
                        }
                    catch (Exception e)
                    {
                        _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                        return Page();
                    }
                    SelectedLeftAssets = new List<Asset>();
                }
                
            }

            if (SelectedRightAssets.Count != 0)
            {
                bool exist = false;
                foreach (var item in SelectedRightAssets)
                {
                    exist = LeftDataSource.Any(e => e.AssetId == item.AssetId);
                    if (!exist)
                    {
                        break;
                    }
                }
                if (exist)
                {
                    //checkIn Right Assets To Store 
                    AssetMovement RightCheckInMovement = null;
                    if (LeftActionTypeId == 1)
                    {
                        
                        foreach (var item in LeftEmployeeDataSource.Distinct())
                        {
                            if (SelectedRightAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = SelectedRightAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                SelectedRightAssets.Remove(removeitem);
                            }
                        }
                    }
                    if (LeftActionTypeId == 2)
                    {
                        foreach (var item in LeftDepartmentDataSource.Distinct()) 
                        {
                            if (SelectedRightAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = SelectedRightAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                SelectedRightAssets.Remove(removeitem);
                            }
                        }
                    }
                    if (RightActionTypeId == 1)
                    {
                        RightCheckInMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 2,
                            EmpolyeeID = RightEmployeeId,
                            ActionTypeId = 1,
                            DepartmentId = RightDepartmentId,
                            LocationId = RightLocationId,
                            TransactionDate = DateTime.Now,
                            StoreId = RightStoreId,
                        };
                    }
                    else if (RightActionTypeId == 2)
                    {
                        RightCheckInMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 2,
                            DepartmentId = RightDepartmentId,
                            ActionTypeId = 2,
                            LocationId = RightLocationId,
                            TransactionDate = DateTime.Now,
                            StoreId = RightStoreId,
                        };
                    }

                    RightCheckInMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                    string RightCheckInDirectionTitle = "Direction Title : ";
                    string RightCheckInTransDate = "Transaction Date : ";
                    AssetMovementDirection RightCheckInDirection = _context.AssetMovementDirections.Find(RightCheckInMovement.AssetMovementDirectionId);
                    string TransactionDate = RightCheckInMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    foreach (var asset in SelectedRightAssets)
                    {
                        asset.AssetStatusId = 1;
                        var UpdatedAsset = _context.Assets.Attach(asset);
                        UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        RightCheckInMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });
                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 16,
                            AssetId = asset.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"{RightCheckInTransDate}{TransactionDate} and {RightCheckInDirectionTitle}{RightCheckInDirection.AssetMovementDirectionTitle} Transfered")
                        };
                        _context.AssetLogs.Add(assetLog);
                    }

                    _context.AssetMovements.Add(RightCheckInMovement);
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                        return Page();
                    }

                    //checkOut Right Assets To Store 

                    AssetMovement RightCheckOutMovement = null;

                    if (LeftActionTypeId == 1)
                    {
                        RightCheckOutMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 1,
                            ActionTypeId = 1,
                            DepartmentId = LeftDepartmentId,
                            LocationId = LeftLocationId,
                            StoreId = RightStoreId,
                            TransactionDate = DateTime.Now,
                            EmpolyeeID = LeftEmployeeId,
                        };

                    }
                    else if (LeftActionTypeId == 2)
                    {
                        RightCheckOutMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 1,
                            ActionTypeId = 2,
                            DepartmentId = LeftDepartmentId,
                            LocationId = LeftLocationId,
                            StoreId = RightStoreId,
                            TransactionDate = DateTime.Now,
                        };
                    }
                    RightCheckOutMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                    string ActionTitle = "Action Title : ";
                    string RightCheckoutTransDate = "Transaction Date : ";
                    string RightCheckOutDirectionTitle = "Direction Title : ";
                    ActionType SelectedActionType = _context.ActionTypes.Find(RightCheckOutMovement.ActionTypeId);
                    AssetMovementDirection RightCheckOutDirection = _context.AssetMovementDirections.Find(RightCheckOutMovement.AssetMovementDirectionId);
                    string CheckoutTransactionDate = RightCheckOutMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    foreach (var asset in SelectedRightAssets)
                    {
                        asset.AssetStatusId = 2;
                        var UpdatedAsset = _context.Assets.Attach(asset);
                        UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        RightCheckOutMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });

                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 17,
                            AssetId = asset.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"{RightCheckoutTransDate}{TransactionDate} and {ActionTitle}{SelectedActionType.ActionTypeTitle} and {RightCheckOutDirectionTitle}{RightCheckOutDirection.AssetMovementDirectionTitle} Transfered")
                        };
                        _context.AssetLogs.Add(assetLog);
                    }
                    _context.AssetMovements.Add(RightCheckOutMovement);
                    try
                    {
                        _context.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                        return Page();
                    }

                }
                SelectedRightAssets = new List<Asset>();

            }
            _toastNotification.AddSuccessToastMessage("Transaction Added Successfully");
            return Page();
        }
              
    }
           
}

    
