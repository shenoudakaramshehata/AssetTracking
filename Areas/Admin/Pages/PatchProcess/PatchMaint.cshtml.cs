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
    public class PatchMaintModel : PageModel
    {
        [BindProperty]
        public AssetMaintainance assetMaintainance { set; get; }
        private readonly AssetContext _context;
        public static List<Asset> SelectedAssets = new List<Asset>();
        private readonly IToastNotification _toastNotification;
        public PatchMaintModel(AssetContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            assetMaintainance = new AssetMaintainance();
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
            if (assetMaintainance.AssetMaintainanceDateCompleted<assetMaintainance.ScheduleDate)
            {
                ModelState.AddModelError("", "Schedule Date Must be less than Completed Date..");
                SelectedAssets = null;
                return Page();
            }
            if (assetMaintainance.TechnicianId ==null)
            {
                ModelState.AddModelError("", "Technican Name Is Required..");
                SelectedAssets = null;
                return Page();
            }
            if ( assetMaintainance.MaintainanceStatusId == null)
            {
                ModelState.AddModelError("", "Status  Name Is Required..");
                SelectedAssets = null;
                return Page();
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
                if (assetMaintainance.AssetMaintainanceFrequencyId ==2)
                {
                    assetMaintainance.MonthlyDay = null;
                    assetMaintainance.MonthlyPeriod = null;
                    assetMaintainance.YearlyDay = null;
                    assetMaintainance.MonthId = null;
                    if (assetMaintainance.WeeklyPeriod==null||assetMaintainance.WeekDayId==null)
                    {
                        ModelState.AddModelError("", "Week Frequency Informations Is Required..");
                        SelectedAssets = null;
                        return Page();
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
                        ModelState.AddModelError("", "Month Frequency Informations Is Required..");
                        SelectedAssets = null;
                        return Page();
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
                        ModelState.AddModelError("", "Year Frequency Informations Is Required..");
                        SelectedAssets = null;
                        return Page();
                    }
                }
            }
            if (ModelState.IsValid)
            {
                string DueDate = assetMaintainance.AssetMaintainanceDueDate?.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                string CompletedDate = assetMaintainance.AssetMaintainanceDateCompleted?.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                if (SelectedAssets!=null)
                {
                    if (SelectedAssets.Count != 0)
                    {
                        foreach (var asset in SelectedAssets)
                        {
                            if (asset.AssetStatusId == 9)
                            {
                                _toastNotification.AddErrorToastMessage(asset.AssetTagId + "Already in Maintainance");
                                return Page();
                            }
                            AssetMaintainance assetMaintainanceObj = new AssetMaintainance
                            {
                                AssetMaintainanceDetails = assetMaintainance.AssetMaintainanceDetails,
                                AssetMaintainanceDateCompleted = assetMaintainance.AssetMaintainanceDateCompleted,
                                AssetMaintainanceDueDate = DateTime.Now,
                                AssetMaintainanceFrequencyId = assetMaintainance.AssetMaintainanceFrequencyId,
                                MaintainanceStatusId = assetMaintainance.MaintainanceStatusId,
                                AssetMaintainanceRepairesCost = assetMaintainance.AssetMaintainanceRepairesCost,
                                AssetMaintainanceTitle = assetMaintainance.AssetMaintainanceTitle,
                                MonthId = assetMaintainance.MonthId,
                                AssetMaintainanceId = assetMaintainance.AssetMaintainanceId,
                                WeekDayId = assetMaintainance.WeekDayId,
                                WeeklyPeriod = assetMaintainance.WeeklyPeriod,
                                MonthlyPeriod = assetMaintainance.MonthlyPeriod,
                                AssetMaintainanceRepeating = assetMaintainance.AssetMaintainanceRepeating,
                                MonthlyDay = assetMaintainance.MonthlyDay,
                                YearlyDay = assetMaintainance.YearlyDay,
                                AssetId = asset.AssetId,
                                TechnicianId = assetMaintainance.TechnicianId,
                                ScheduleDate = assetMaintainance.ScheduleDate

                            };
                            asset.AssetStatusId = 9;
                            var UpdatedAsset = _context.Assets.Attach(asset);
                            UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                            AssetLog assetLog = new AssetLog()
                            {
                                ActionLogId = 15,
                                AssetId = asset.AssetId,
                                ActionDate = DateTime.Now,
                                Remark = string.Format($"Maintainance Asset with Title {assetMaintainance.AssetMaintainanceTitle} and DueDate {DueDate} and Completed Date {CompletedDate}")
                            };
                            _context.AssetLogs.Add(assetLog);
                            _context.AssetMaintainances.Add(assetMaintainanceObj);
                        }
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            SelectedAssets = null;
                            _toastNotification.AddErrorToastMessage("Something went Error,Try again");
                            return Page();
                        }
                        SelectedAssets = null;
                        _toastNotification.AddSuccessToastMessage("Asset Maintainance Patched Added successfully");
                        return RedirectToPage();
                    }
                }
                SelectedAssets = null;
                _toastNotification.AddErrorToastMessage("Please Select at Least one Asset");
                return Page();
            }
            SelectedAssets = null;
            _toastNotification.AddErrorToastMessage("Something went Error,Try again");
            return Page();
        }
    }
}

