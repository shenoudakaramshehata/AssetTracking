using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssetProject.Areas.Admin.Pages.AssetManagment
{
    [Authorize]
    public class AssetProfileModel : PageModel
    {
        AssetContext Context;
        public Asset Asset { set; get; }
        public string AssetPhoto { set; get; }
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public AssetMovement AssetMovementModel { set; get; }
        public AssetRepair AssetrepairModel { set; get; }
        public AssetMaintainance AssetMaintainance { set; get; }
        [BindProperty]
        public int AssetId { set; get; }
        public Asset asset { get; set; }
        public Tenant tenant { set; get; }
        UserManager<ApplicationUser> UserManger;
        public AssetProfileModel(AssetContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            Context = context;
            _hostEnvironment = hostEnvironment;
            UserManger = userManager;
            _toastNotification = toastNotification;
            asset = new Asset();
            AssetMaintainance = new AssetMaintainance();
        }
        public async Task<IActionResult> OnGetAsync(int AssetId)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = Context.Tenants.Find(user.TenantId);
            Asset = Context.Assets.Where(a => a.AssetId == AssetId).Include(a => a.Item).Include(a => a.DepreciationMethod).Include(a => a.AssetStatus).FirstOrDefault();
            if (Asset == null)
            {
                return Redirect("../NotFound");

            }
            if (Asset.TenantId != tenant.TenantId)
            {
                return Redirect("../NotFound");
            }
            AssetPhoto = "/" + Asset.Photo;
            AssetId = Asset.AssetId;
            return Page();
        }

        public async Task<IActionResult> OnPostAddAssetPhotot(IFormFile file, AssetPhotos photos)
        {
            //var assetobj = Context.Assets.Where(e => e.AssetId == photos.AssetId).Include(e => e.AssetStatus).FirstOrDefault();

            //if (assetobj.AssetStatusId == 4 || assetobj.AssetStatusId == 5 || assetobj.AssetStatusId == 7 || assetobj.AssetStatusId == 8)
            //{
            //    _toastNotification.AddErrorToastMessage("Can’t Add Asset Photos becasuse Asset Now is" + "" + assetobj.AssetStatus.AssetStatusTitle );
            //    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = photos.AssetId });
            //}
            if (file != null)
            {
                string folder = "Images/AssetPhotos/";
                photos.PhotoUrl = await UploadImage(folder, file);
            }
            if (photos.AssetId != 0)
            {
                Context.AssetPhotos.Add(photos);
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 5,
                    AssetId = photos.AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($" Description : {photos.Remarks} ")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Photo Added successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = photos.AssetId });
            }
            _toastNotification.AddErrorToastMessage("Asset Photo Not ADDED ,Try again");
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = photos.AssetId });
        }
        public IActionResult OnGetDeletePhoto(int AssetId, int AssetPhotoId)
        {
            var Result = Context.AssetPhotos.Where(a => a.AssetId == AssetId && a.AssetPhotoId == AssetPhotoId).FirstOrDefault();

            Context.AssetPhotos.Remove(Result);
            AssetLog assetLog = new AssetLog()
            {
                ActionLogId = 9,
                AssetId = AssetId,
                ActionDate = DateTime.Now,
            };
            Context.AssetLogs.Add(assetLog);
            Context.SaveChanges();
            if (Result.PhotoUrl != null)
            {
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, Result.PhotoUrl);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
            }

            return new JsonResult(Result);
        }
        public async Task<IActionResult> OnPostAddAssetDocument(AssetDocument instance, IFormFile file)
        {
            //var assetobj = Context.Assets.Where(e => e.AssetId == instance.AssetId).Include(e => e.AssetStatus).FirstOrDefault();

            //if (assetobj.AssetStatusId == 4 || assetobj.AssetStatusId == 5 || assetobj.AssetStatusId == 7 || assetobj.AssetStatusId == 8)
            //{
            //    _toastNotification.AddErrorToastMessage("Can’t Add Asset Document becasuse Asset Now is" + "" + assetobj.AssetStatus.AssetStatusTitle);
            //    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = instance.AssetId });
            //}
            if (file != null)
            {

                string folder = "Documents/AssetDocuments/";
                instance.DocumentType = await UploadImage(folder, file);
            }
            Context.assetDocuments.Add(instance);
            AssetLog assetLog = new AssetLog()
            {
                ActionLogId = 4,
                AssetId = instance.AssetId,
                ActionDate = DateTime.Now,
                Remark = string.Format($"Document Name : {instance.DocumentName} ")
            };
            Context.AssetLogs.Add(assetLog);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Document Added successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = instance.AssetId });
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went ");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = instance.AssetId });
            }

        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }

        public IActionResult OnpostAddAssetContract(AssetContract assetcontract)
        {
            var assetobj = Context.Assets.Where(e => e.AssetId == assetcontract.AssetId).Include(e => e.AssetStatus).FirstOrDefault();
            var contractobj = Context.AssetContracts.Where(a => a.ContractId == assetcontract.ContractId && a.AssetId == assetcontract.AssetId).FirstOrDefault();
            if (contractobj != null)
            {
                _toastNotification.AddErrorToastMessage("This contract was previously added to this asset..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetcontract.AssetId });
            }
            if (assetobj.AssetStatusId == 4 || assetobj.AssetStatusId == 5 || assetobj.AssetStatusId == 7 || assetobj.AssetStatusId == 8)
            {
                _toastNotification.AddErrorToastMessage("Can’t Add Asset Contract becasuse Asset Now is" + "" + assetobj.AssetStatus.AssetStatusTitle);
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetcontract.AssetId });
            }
            if (assetcontract.ContractId != 0 && assetcontract.AssetId != 0)
            {
                var Assetcont = new AssetContract { AssetId = assetcontract.AssetId, ContractId = assetcontract.ContractId };
                Context.AssetContracts.Add(Assetcont);

                string ContractTitle = "Contract Title : ";
                string ContractSDate = "Contract Start Date : ";
                string ContractEDate = "Contract End Date : ";
                Contract SelectedContract = Context.Contracts.Find(assetcontract.ContractId);
                string ContractStartDate = SelectedContract.StartDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                string ContractEndDate = SelectedContract.EndDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 2,
                    AssetId = assetcontract.AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"{ContractTitle}{SelectedContract.Title} , {ContractSDate}{ContractStartDate} and {ContractEDate}{ContractEndDate}")
                };
                Context.AssetLogs.Add(assetLog);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Link Contract Added Successfully");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetcontract.AssetId });
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetcontract.AssetId });
                }
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetcontract.AssetId });
        }
        public IActionResult OnpostAddAssetInsurance(AssetsInsurance assetsInsurance)
        {
            var assetobj = Context.Assets.Where(e => e.AssetId == assetsInsurance.AssetId).Include(e => e.AssetStatus).FirstOrDefault();
            var insuranceobj = Context.AssetsInsurances.Where(a => a.InsuranceId == assetsInsurance.InsuranceId && a.AssetId == assetsInsurance.AssetId).FirstOrDefault();
            if (insuranceobj != null)
            {
                _toastNotification.AddErrorToastMessage("This Insurance was previously added to this asset..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetsInsurance.AssetId });
            }
            if (assetobj.AssetStatusId == 4 || assetobj.AssetStatusId == 5 || assetobj.AssetStatusId == 7 || assetobj.AssetStatusId == 8)
            {
                _toastNotification.AddErrorToastMessage("Can’t Add Asset Insuurance becasuse Asset Now is" + "" + assetobj.AssetStatus.AssetStatusTitle);
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetsInsurance.AssetId });
            }
            if (assetsInsurance.InsuranceId != 0 && assetsInsurance.AssetId != 0)
            {
                AssetsInsurance AssetIns = new AssetsInsurance { AssetId = assetsInsurance.AssetId, InsuranceId = assetsInsurance.InsuranceId };
                Context.AssetsInsurances.Add(AssetIns);
                string InsuranceTitle = "Insurance Title : ";
                string InsuranceCompany = "Insurance Company : ";
                Insurance SelectedInsurance = Context.Insurances.Find(assetsInsurance.InsuranceId);
                string InsuranceTit = SelectedInsurance.Title;
                string InsuranceComp = SelectedInsurance.InsuranceCompany;

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 3,
                    AssetId = assetsInsurance.AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"{InsuranceTitle}{InsuranceTit} and {InsuranceCompany}{InsuranceComp} ")
                };
                Context.AssetLogs.Add(assetLog);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Link Insurance Added Successfully");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetsInsurance.AssetId });
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Link Insurance Added Successfully");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetsInsurance.AssetId });
                }
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetsInsurance.AssetId });
        }

        public IActionResult OnPostAddAssetCheckOut(AssetMovement assetMovement)
        {

            if (ModelState.IsValid && assetMovement.ActionTypeId != null
                && assetMovement.DepartmentId != null && assetMovement.LocationId != null
                )
            {
                if (assetMovement.ActionTypeId == 1)
                {
                    if (assetMovement.EmpolyeeID == null)
                    {
                        _toastNotification.AddErrorToastMessage("Asset Movement Not Added ,Try again");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
                    }
                }
                var asset = Context.Assets.Find(AssetId);
                var LastAssetMovementDetails = Context.AssetMovementDetails.Where(a => a.AssetId == AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                if (LastAssetMovementDetails != null)
                {
                    var LastAssetMovement = Context.AssetMovements.Find(LastAssetMovementDetails.AssetMovementId);

                    if (LastAssetMovement != null)
                    {
                        assetMovement.StoreId = LastAssetMovement.StoreId;
                    }
                }
                else
                {
                    assetMovement.StoreId = asset.StoreId;

                }
                assetMovement.TransactionDate = DateTime.Now;
                assetMovement.AssetMovementDirectionId = 1;
                asset.AssetStatusId = 2;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                assetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                assetMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = AssetId, Remarks = "" });
                Context.AssetMovements.Add(assetMovement);
                string ActionTitle = "Action Title : ";
                string TransDate = "Transaction Date : ";
                string DirectionTitle = "Direction Title : ";
                ActionType SelectedActionType = Context.ActionTypes.Find(assetMovement.ActionTypeId);
                AssetMovementDirection Direction = Context.AssetMovementDirections.Find(assetMovement.AssetMovementDirectionId);
                string TransactionDate = assetMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 17,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"{TransDate}{TransactionDate} and {ActionTitle}{SelectedActionType.ActionTypeTitle} and {DirectionTitle}{Direction.AssetMovementDirectionTitle}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Movement Added Successfully");
                return RedirectToPage("/ReportsManagement/CheckoutFormRPT", new { AssetMovement = assetMovement.AssetMovementId });
            }
            _toastNotification.AddErrorToastMessage("Asset Movement Not Added ,Try again");
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }

        public IActionResult OnPostAddAssetCheckIn(AssetMovement assetMovement)
        {

            if (ModelState.IsValid && assetMovement.StoreId != null)
            {
                var asset = Context.Assets.Find(AssetId);
                //var LastAssetMovementDetails = Context.AssetMovementDetails.Where(a => a.AssetId == AssetId).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                //if (LastAssetMovementDetails != null)
                //{
                //    var LastAssetMovement = Context.AssetMovements.Find(LastAssetMovementDetails.AssetMovementId);
                //    assetMovement.ActionTypeId = LastAssetMovement.ActionTypeId;
                //    assetMovement.DepartmentId = LastAssetMovement.DepartmentId;
                //    assetMovement.LocationId = LastAssetMovement.LocationId;
                //    assetMovement.EmpolyeeID = LastAssetMovement.EmpolyeeID;
                //}
                assetMovement.AssetMovementDirectionId = 2;
                assetMovement.TransactionDate = DateTime.Now;
                asset.AssetStatusId = 1;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                assetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                assetMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = AssetId, Remarks = "" });
                Context.AssetMovements.Add(assetMovement);
                string DirectionTitle = "Direction Title : ";
                string TransDate = "Transaction Date : ";
                AssetMovementDirection Direction = Context.AssetMovementDirections.Find(assetMovement.AssetMovementDirectionId);
                string TransactionDate = assetMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 16,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"{TransDate}{TransactionDate} and {DirectionTitle}{Direction.AssetMovementDirectionTitle}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Movement Added Successfully");
                return RedirectToPage("/ReportsManagement/CheckInFormRPT", new { AssetMovement = assetMovement.AssetMovementId });
            }
            _toastNotification.AddErrorToastMessage("Asset Movement Not Added ,Try again");
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }

        public IActionResult OnPostAddAssetRepair(AssetRepair assetRepair)
        {
            if (ModelState.IsValid && assetRepair.TechnicianId != 0)
            {

                //Update Asset Status
                var asset = Context.Assets.Find(AssetId);
                asset.AssetStatusId = 3;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                assetRepair.AssetRepairDetails = new List<AssetRepairDetails>();
                assetRepair.AssetRepairDetails.Add(new AssetRepairDetails { AssetId = AssetId, Remarks = "" });
                Context.AssetRepairs.Add(assetRepair);
                Technician technician = Context.Technicians.Find(assetRepair.TechnicianId);
                string ScheduleDate = "Schedule Date : ";
                string CompletedDate = "Completed Date : ";
                string ScheduleD = assetRepair.ScheduleDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                string CompletedD = assetRepair.CompletedDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 13,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Repair Asset Asigned to {technician.FullName} with {ScheduleDate}{ScheduleD} and {CompletedDate}{CompletedD}")
                };

                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Repair Added Successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
            }
            _toastNotification.AddErrorToastMessage("Asset Repair Not Added ,Try again");
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }
        public IActionResult OnpostAddAssetLost(AssetLost assetlost)
        {
            if (ModelState.IsValid && assetlost.DateLost.Date.Day > 1)
            {
                var asset = Context.Assets.Find(AssetId);
                assetlost.AssetLostDetails = new List<AssetLostDetails>();
                assetlost.AssetLostDetails.Add(new AssetLostDetails() { AssetId = AssetId, Remarks = "" });
                Context.AssetLosts.Add(assetlost);

                asset.AssetStatusId = 4;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                string LostDate = assetlost.DateLost.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 14,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Asset Lost Date {LostDate}")
                };
                Context.AssetLogs.Add(assetLog);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Asset Lost Added successfully");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
                }
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }

        public IActionResult OnPostAddDisposeAsset(DisposeAsset disposeAsset)
        {
            if (ModelState.IsValid && disposeAsset.DateDisposed.Date.Day > 1)
            {
                var asset = Context.Assets.Find(AssetId);
                disposeAsset.AssetDisposeDetails = new List<AssetDisposeDetails>();
                disposeAsset.AssetDisposeDetails.Add(new AssetDisposeDetails() { AssetId = AssetId, Remarks = "" }
                );
                Context.DisposeAssets.Add(disposeAsset);
                asset.AssetStatusId = 5;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                string DisposeDate = "Dispose Date : ";
                string DisposeTo = "Disposed To  : ";
                string DisposeD = disposeAsset.DateDisposed.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                string DisposeFor = disposeAsset.DisposeTo;
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 11,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"{DisposeDate}{DisposeD} && {DisposeTo}{DisposeFor}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Dispose Asset Added successfully");

                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }

        public IActionResult OnpostAddAssetLeasing(AssetLeasing AssetLeasing)
        {
            if (ModelState.IsValid && AssetLeasing.StartDate.Date < AssetLeasing.EndDate)
            {

                var asset = Context.Assets.Find(AssetId);
                AssetLeasing.AssetLeasingDetails = new List<AssetLeasingDetails>();
                AssetLeasing.AssetLeasingDetails.Add(new AssetLeasingDetails() { AssetId = AssetId, Remarks = "" });
                Context.AssetLeasings.Add(AssetLeasing);

                asset.AssetStatusId = 6;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                string StartLeasingDate = AssetLeasing.StartDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                string EndLeasingDate = AssetLeasing.EndDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 15,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Leasing Asset Date is Between {StartLeasingDate} and {EndLeasingDate}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Leasing Added successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new
                {
                    AssetId = AssetId
                });
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }


        public IActionResult OnpostAddAssetsell(SellAsset Sellasset)
        {
            if (ModelState.IsValid && Sellasset.SaleDate.Date.Day > 1)
            {
                var asset = Context.Assets.Find(AssetId);
                Sellasset.AssetSellDetails = new List<AssetSellDetails>();
                Sellasset.AssetSellDetails.Add(new AssetSellDetails() { AssetId = AssetId, Remarks = "" }
                );
                Context.sellAssets.Add(Sellasset);
                asset.AssetStatusId = 7;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                string SaleD = Sellasset.SaleDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 10,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Sold Asset To {Sellasset.SoldTo} in Date {SaleD} With Amoun {Sellasset.SaleAmount}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Selling Added successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }

        public IActionResult OnpostAddAssetBroken(AssetBroken assetBroken)
        {

            if (ModelState.IsValid)
            {
                var asset = Context.Assets.Find(AssetId);
                assetBroken.AssetBrokenDetails = new List<AssetBrokenDetails>();
                assetBroken.AssetBrokenDetails.Add(new AssetBrokenDetails() { AssetId = AssetId, Remarks = "" });
                Context.assetBrokens.Add(assetBroken);

                asset.AssetStatusId = 8;
                var UpdatedAsset = Context.Assets.Attach(asset);
                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                string BrockenDate = assetBroken.DateBroken.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 12,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Brocken Asset Date {BrockenDate}")
                };
                Context.AssetLogs.Add(assetLog);
                try
                {
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Asset Broken Added successfully");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                    return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
                }
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = AssetId });
        }

        public IActionResult OnPostAddAssetMaintainance(AssetMaintainance assetMaintainance)
        {
            var assetobj = Context.Assets.Where(e => e.AssetId == assetMaintainance.AssetId).Include(e => e.AssetStatus).FirstOrDefault();

            if (assetobj.AssetStatusId != 1 && assetobj.AssetStatusId != 2)
            {
                _toastNotification.AddErrorToastMessage("Asset Now is" + assetobj.AssetStatus.AssetStatusTitle);
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.AssetMaintainanceDateCompleted < assetMaintainance.ScheduleDate)
            {
                _toastNotification.AddErrorToastMessage("Schedule Date Must be less than Completed Date..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.TechnicianId == null)
            {
                _toastNotification.AddErrorToastMessage("Technican Name Is Required..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.MaintainanceStatusId == null)
            {
                _toastNotification.AddErrorToastMessage("Status  Name Is Required..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.MaintainanceStatusId != 5)
            {
                assetMaintainance.AssetMaintainanceDateCompleted = null;
            }
            if (!assetMaintainance.AssetMaintainanceRepeating)
            {
                assetMaintainance.AssetMaintainanceFrequencyId = null;
                assetMaintainance.WeekDayId = null;
                assetMaintainance.WeeklyPeriod = null;
                assetMaintainance.MonthlyDay = null;
                assetMaintainance.MonthlyPeriod = null;
                assetMaintainance.YearlyDay = null;
                assetMaintainance.MonthId = null;
            }
            else
            {
                if (assetMaintainance.AssetMaintainanceFrequencyId == 1)
                {
                    assetMaintainance.WeekDayId = null;
                    assetMaintainance.WeeklyPeriod = null;
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                }
                if (assetMaintainance.AssetMaintainanceFrequencyId == 2)
                {
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                    if (assetMaintainance.WeeklyPeriod == null || assetMaintainance.WeekDayId == null)
                    {
                        _toastNotification.AddErrorToastMessage("Week Frequency Informations Is Required..");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
                    }
                }
                if (assetMaintainance.AssetMaintainanceFrequencyId == 3)
                {
                    assetMaintainance.WeekDayId = null;
                    assetMaintainance.WeeklyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                    if (assetMaintainance.MonthlyPeriod == null || assetMaintainance.MonthlyDay == null)
                    {
                        _toastNotification.AddErrorToastMessage("Month Frequency Informations Is Required..");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
                    }
                }
                if (assetMaintainance.AssetMaintainanceFrequencyId == 4)
                {
                    assetMaintainance.WeekDayId = null;
                    assetMaintainance.WeeklyPeriod = null;
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    if (assetMaintainance.YearlyDay == null || assetMaintainance.MonthId == null)
                    {
                        _toastNotification.AddErrorToastMessage("Year Frequency Informations Is Required..");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
                    }
                }
            }
            if (ModelState.IsValid)
            {
                assetMaintainance.AssetMaintainanceDueDate = DateTime.Now;
                Context.AssetMaintainances.Add(assetMaintainance);

                //assetobj.AssetStatusId = 9;
                //var UpdatedAsset = Context.Assets.Attach(assetobj);
                //UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                string DueDate = assetMaintainance.AssetMaintainanceDueDate?.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                string CompletedDate = assetMaintainance.AssetMaintainanceDateCompleted?.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 18,
                    AssetId = AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Maintainance Asset with Title {assetMaintainance.AssetMaintainanceTitle} and DueDate {DueDate} and Completed Date {CompletedDate}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Maintainance Added successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            _toastNotification.AddErrorToastMessage("Asset Maintainance Not Added ,Try again");
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
        }

        public IActionResult OnpostDeattachAssetContract(AssetContract assetContract)
        {
            AssetContract _assetContract = Context.AssetContracts.Where(e => e.AssetContractID == assetContract.AssetContractID && e.AssetId == assetContract.AssetId).FirstOrDefault();


            Context.AssetContracts.Remove(_assetContract);


            Contract contract = Context.Contracts.Find(assetContract.ContractId);

            AssetLog assetLog = new AssetLog()
            {
                ActionLogId = 6,
                AssetId = assetContract.AssetId,
                ActionDate = DateTime.Now,
                Remark = string.Format($"Dettached Asset Contract With Contract Name : {contract.Title} and Contract Number : {contract.ContractNo}")
            };
            Context.AssetLogs.Add(assetLog);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Contract Dettached Succeffully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetContract.AssetId });
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetContract.AssetId });
            }

        }
        public IActionResult OnpostDeattachAssetInsurance(AssetsInsurance assetInsurance)
        {

            AssetsInsurance _assetInsurance = Context.AssetsInsurances.Where(e => e.AssetsInsuranceId == assetInsurance.AssetsInsuranceId && e.AssetId == assetInsurance.AssetId).FirstOrDefault();
            Context.AssetsInsurances.Remove(_assetInsurance);
            Insurance insurance = Context.Insurances.Find(assetInsurance.InsuranceId);

            AssetLog assetLog = new AssetLog()
            {
                ActionLogId = 7,
                AssetId = assetInsurance.AssetId,
                ActionDate = DateTime.Now,
                Remark = string.Format($"Dettached Asset Insurance With Insurance Name : {insurance.Title} and Insurance Company : {insurance.InsuranceCompany}")
            };
            Context.AssetLogs.Add(assetLog);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Insurance Dettached Succeffully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetInsurance.AssetId });
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetInsurance.AssetId });
            }
        }




        public IActionResult OnpostDeattachAssetDocument(AssetDocument assetDocument)
        {
            AssetDocument _assetDocument = Context.assetDocuments.Where(e => e.AssetDocumentId == assetDocument.AssetDocumentId && e.AssetId == assetDocument.AssetId).FirstOrDefault();
            string AssetDocName = _assetDocument.DocumentName;


            Context.assetDocuments.Remove(_assetDocument);


            AssetLog assetLog = new AssetLog()
            {
                ActionLogId = 8,
                AssetId = assetDocument.AssetId,
                ActionDate = DateTime.Now,
                Remark = string.Format($"Dettached Asset Document With Document Name : {AssetDocName} ")
            };
            Context.AssetLogs.Add(assetLog);
            try
            {
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Document Dettached Succeffully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetDocument.AssetId });
            }

            catch(Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetDocument.AssetId });
            }
        }
        public IActionResult OnPostAddAssetWarranty(AssetWarranty assetWarranty)
        {
            var assetobj = Context.Assets.Where(e => e.AssetId == assetWarranty.AssetId).Include(e => e.AssetStatus).FirstOrDefault();
            if (assetobj.AssetStatusId == 4 || assetobj.AssetStatusId == 5 || assetobj.AssetStatusId == 7 || assetobj.AssetStatusId == 8)
            {
                _toastNotification.AddErrorToastMessage("Can’t Add Asset asset Warranty becasuse Asset Now is" + " " + assetobj.AssetStatus.AssetStatusTitle);
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetWarranty.AssetId });
            }
            if (assetWarranty.AssetId != 0)
            {
                try
                {
                    AssetWarranty AssetWar = new AssetWarranty { AssetId = assetWarranty.AssetId, Length = assetWarranty.Length, Notes = assetWarranty.Notes, ExpirationDate = assetWarranty.ExpirationDate, };
                    Context.AssetWarranties.Add(AssetWar);
                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 20,
                        AssetId = assetWarranty.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format("Create Warranty")
                    };
                    Context.AssetLogs.Add(assetLog);
                    Context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Link Warranty Added Successfully");
                }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("SomeThing went Wrong..");
                }
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetWarranty.AssetId });
            }

            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetWarranty.AssetId });
        }

        public IActionResult OnPostDeattachAssetWarranty(AssetWarranty assetwarranty)
        {
            AssetWarranty _assetwarranty = Context.AssetWarranties.Where(e => e.WarrantyId == assetwarranty.WarrantyId).FirstOrDefault();
            try
            {
                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 21,
                    AssetId = _assetwarranty.AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Dettached Asset Warranty With Warranty Length: {_assetwarranty.Length} and Expiration Date : {_assetwarranty.ExpirationDate}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.AssetWarranties.Remove(_assetwarranty);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Asset Warranty Dettached Succeffully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Some Thing Went Error");
            }
            Context.SaveChanges();
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetwarranty.AssetId });

        }

        public IActionResult OnGetEditMiantainance(int maintainanceId)
        {
            var maintainance = Context.AssetMaintainances.Find(maintainanceId);
            return new JsonResult(maintainance);
        }
        public IActionResult OnPostEditAssetMaintainance(AssetMaintainance assetMaintainance)
        {
            var assetobj = Context.Assets.Find(assetMaintainance.AssetId);
            if (assetobj.AssetStatusId != 9)
            {
                _toastNotification.AddErrorToastMessage("Cannot Edit Previous Asset Maintainance..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            //else if (assetobj.AssetStatusId==9)
            //{
            //    var lastassetMaintainance = Context.AssetMaintainances.Where(e => e.AssetId == assetMaintainance.AssetId).OrderByDescending(e => e.AssetMaintainanceId).FirstOrDefault();
            //    if (assetMaintainance.AssetMaintainanceId != lastassetMaintainance.AssetMaintainanceId)
            //    {
            //        _toastNotification.AddErrorToastMessage("Cannot Edit Previous Asset Maintainance..");
            //        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            //    }
            //}
            if (assetMaintainance.AssetMaintainanceDateCompleted < assetMaintainance.ScheduleDate)
            {
                _toastNotification.AddErrorToastMessage("Schedule Date Must be less than Completed Date..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.TechnicianId == null)
            {
                _toastNotification.AddErrorToastMessage("Technican Name Is Required..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.MaintainanceStatusId == null)
            {
                _toastNotification.AddErrorToastMessage("Status  Name Is Required..");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            if (assetMaintainance.MaintainanceStatusId != 5)
            {
                assetMaintainance.AssetMaintainanceDateCompleted = null;
            }
            //if (assetMaintainance.MaintainanceStatusId == 5 || assetMaintainance.MaintainanceStatusId == 4)
            //{
            //    AssetMovement assetCheckin = new AssetMovement()
            //    {
            //        AssetMovementDirectionId = 2,
            //        TransactionDate = DateTime.Now,
            //        StoreId = null
            //    };
            //    assetCheckin.AssetMovementDetails= new List<AssetMovementDetails>();
            //    assetCheckin.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = assetMaintainance.AssetId, Remarks = "" });
            //    Context.AssetMovements.Add(assetCheckin);
            //    assetobj.AssetStatusId = 1;
            //    var UpdatedAsset = Context.Assets.Attach(assetobj);
            //    UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //}
            if (!assetMaintainance.AssetMaintainanceRepeating)
            {
                assetMaintainance.AssetMaintainanceFrequencyId = null;
                assetMaintainance.WeekDayId = null;
                assetMaintainance.WeeklyPeriod = null;
                assetMaintainance.MonthlyDay = null;
                assetMaintainance.MonthlyPeriod = null;
                assetMaintainance.YearlyDay = null;
                assetMaintainance.MonthId = null;
            }
            else
            {
                if (assetMaintainance.AssetMaintainanceFrequencyId == 1)
                {
                    assetMaintainance.WeekDayId = null;
                    assetMaintainance.WeeklyPeriod = null;
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                }
                if (assetMaintainance.AssetMaintainanceFrequencyId == 2)
                {
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                    if (assetMaintainance.WeeklyPeriod == null || assetMaintainance.WeekDayId == null)
                    {
                        _toastNotification.AddErrorToastMessage("Week Frequency Informations Is Required..");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
                    }
                }
                if (assetMaintainance.AssetMaintainanceFrequencyId == 3)
                {
                    assetMaintainance.WeekDayId = null;
                    assetMaintainance.WeeklyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                    if (assetMaintainance.MonthlyPeriod == null || assetMaintainance.MonthlyDay == null)
                    {
                        _toastNotification.AddErrorToastMessage("Month Frequency Informations Is Required..");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
                    }
                }
                if (assetMaintainance.AssetMaintainanceFrequencyId == 4)
                {
                    assetMaintainance.WeekDayId = null;
                    assetMaintainance.WeeklyPeriod = null;
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    if (assetMaintainance.YearlyDay == null || assetMaintainance.MonthId == null)
                    {
                        _toastNotification.AddErrorToastMessage("Year Frequency Informations Is Required..");
                        return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
                    }
                }
            }
            if (ModelState.IsValid)
            {
                var UpdatedMaintainance = Context.AssetMaintainances.Attach(assetMaintainance);
                UpdatedMaintainance.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var Status = Context.MaintainanceStatuses.Find(assetMaintainance.MaintainanceStatusId).MaintainanceStatusTitle;

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 22,
                    AssetId = assetMaintainance.AssetId,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"Edit Asset Maintainance with Status {Status}")
                };
                Context.AssetLogs.Add(assetLog);
                Context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Maintainance Edited successfully");
                return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
            }
            _toastNotification.AddErrorToastMessage("Asset Maintainance Not Added ,Try again");
            return RedirectToPage("/AssetManagment/AssetProfile", new { AssetId = assetMaintainance.AssetId });
        }
    }
}