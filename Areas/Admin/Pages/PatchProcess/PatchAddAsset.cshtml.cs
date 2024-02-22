using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AssetProject.Areas.Admin.Pages.PatchProcess
{
    public class PatchAddAssetModel : PageModel
    {
        [BindProperty]
        public Asset Asset { set; get; }
        private readonly AssetContext _context;
        public  List<Asset> Assets = new List<Asset>();
        public  List<Asset> DataSource = new List<Asset>();

        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PatchAddAssetModel(AssetContext context, IWebHostEnvironment webHostEnvironment, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            Asset = new Asset();
           _toastNotification = toastNotification;
            UserManger = userManager;
        }
        public void OnGet()
        {
           
        }
        public async Task<IActionResult> OnPost(IFormFile file)
        {
            Assets = new List<Asset>();

            if (Asset.ItemId == 0)
            {
                ModelState.AddModelError("", "Please Select Item");
                return Page();
            }
            if (Asset.StoreId == null)
            {
                ModelState.AddModelError("", "Please Select Store");
                return Page();
            }
            if (Asset.VendorId == null)
            {
                ModelState.AddModelError("", "Please Select Vendor");
                return Page();
            }
            if (Asset.DepreciableAsset)
            {
                if (Asset.DepreciationMethodId == null)
                {
                    ModelState.AddModelError("", "Please Select Depreciation Method");
                    return Page();
                }

            }
            if (!Asset.DepreciableAsset)
            {
                Asset.DepreciableCost = null;
                Asset.DateAcquired = null;
                Asset.DepreciationMethodId = null;
                Asset.SalvageValue = null;
                Asset.AssetLife = null;
            }

            if (ModelState.IsValid)
            {
                for (int i = 0; i < Asset.Quantity; i++)
                {
                    Asset asset = new Asset();
                    if (file != null)
                    {
                        string folder = "Images/AssetPhotos/";
                        asset.Photo = await UploadImage(folder, file);
                    }
                    asset.AssetStatusId = 1;
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await UserManger.FindByIdAsync(userid);
                    tenant = _context.Tenants.Find(user.TenantId);
                    asset.TenantId = tenant.TenantId;
                    asset.PurchaseNo = Asset.PurchaseNo;
                    asset.ItemId = Asset.ItemId;
                    asset.StoreId = Asset.StoreId;
                    asset.VendorId = Asset.VendorId;
                    asset.AssetDescription = Asset.AssetDescription;
                    asset.DepreciableAsset = Asset.DepreciableAsset;
                    asset.AssetCost = Asset.AssetCost;
                    asset.AssetLife = Asset.AssetLife;
                    asset.DateAcquired = Asset.DateAcquired;
                    asset.DepreciationMethodId = Asset.DepreciationMethodId;
                    asset.AssetPurchaseDate = Asset.AssetPurchaseDate;
                    asset.DepreciableCost = Asset.DepreciableCost;
                    asset.SalvageValue = Asset.SalvageValue;
                    
                    _context.Assets.Add(asset);
                    Assets.Add(asset);
                   
                    
                }
               
                try
                {
                    _context.SaveChanges();
                    
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went Error");
                }
                foreach (var item in Assets)
                {
                    string Str = "purchase Date : ";
                    string AssetPurchaseDate = Asset.AssetPurchaseDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 1,
                        AssetId = item.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($"{Str}{AssetPurchaseDate}")
                    };
                    _context.AssetLogs.Add(assetLog);
                }
                try
                {
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Asset Added successfully");
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage("Something went Error");
                }

            }
            return Page();
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
    }
}
