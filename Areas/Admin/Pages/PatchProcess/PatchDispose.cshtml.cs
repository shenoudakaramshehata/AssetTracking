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

namespace AssetProject.Areas.Admin.Pages.PatchProcess
{
    [Authorize]
    public class PatchDisposeModel : PageModel
    {
        [BindProperty]
        public DisposeAsset disposeAsset { set; get; }
        private readonly AssetContext _context;
        public static List<Asset> SelectedAssets = new List<Asset>();
        private readonly IToastNotification _toastNotification;
        public PatchDisposeModel(AssetContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPostFillAssetList([FromBody] List<Asset> assets)
        {

            SelectedAssets = assets;
            return new JsonResult(assets);
        }

        public IActionResult OnPost()
        {

            if (ModelState.IsValid)
            {
                if (SelectedAssets!=null)
                {

                if (SelectedAssets.Count != 0)
                {
                    disposeAsset.AssetDisposeDetails= new List<AssetDisposeDetails>();
                    string DisposeDate = "Dispose Date : ";
                    string DisposeTo = "Disposed To  : ";
                    string DisposeD = disposeAsset.DateDisposed.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    string DisposeFor = disposeAsset.DisposeTo;
                   


                    foreach (var asset in SelectedAssets)
                    {

                        asset.AssetStatusId = 5;
                        var UpdatedAsset = _context.Assets.Attach(asset);
                        UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        disposeAsset.AssetDisposeDetails.Add(new AssetDisposeDetails() { AssetId = asset.AssetId, Remarks = "" });

                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 11,
                            AssetId = asset.AssetId,
                            ActionDate = DateTime.Now,
                             Remark = string.Format($"{DisposeDate}{DisposeD} && {DisposeTo}{DisposeFor}")
                        };
                        _context.AssetLogs.Add(assetLog);
                    }
                    _context.DisposeAssets.Add(disposeAsset);
                  
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                        SelectedAssets = null;
                        return Page();
                    }
                    _toastNotification.AddSuccessToastMessage("Asset Disposd Patched Added successfully");
                    SelectedAssets = null;
                    return RedirectToPage();
                }
                }

                _toastNotification.AddErrorToastMessage("Please Select at Least one Asset");
                SelectedAssets = null;
                return Page();
            }
            _toastNotification.AddErrorToastMessage("Something went Error,Try again");
            SelectedAssets = null;
            return Page();
        }
    }
}
