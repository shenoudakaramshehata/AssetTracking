using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Newtonsoft.Json;
using AssetProject.ViewModel;
using AutoMapper;

namespace AssetProject.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MobileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public static List<Asset> SelectedAssets = new List<Asset>();
        public IHttpContextAccessor _httpContextAccessor { get; set; }
        private readonly IMapper _mapper;
        public AssetContext _context { get; set; }
        public MobileController(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signInManager, AssetContext Context, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _context = Context;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string Email, [FromQuery] string Password)
        {

            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);
                if (result.Succeeded)
                {
                    return Ok(new { Status = "Success", Message = "User Login successfully!", user });
                }
            }
            var invalidResponse = new { status = false };
            return Ok(invalidResponse);
        }
        [HttpGet]
        public IActionResult Search([FromQuery] string Parcode, [FromQuery] int TentantId)
        {
            if (Parcode != null)
            {
                var items = _context.Assets.Where(c =>c.TenantId== TentantId && (c.AssetTagId.Contains(Parcode)
                || c.AssetSerialNo.Contains(Parcode) || c.AssetId.ToString() == Parcode
                )).Select(i => new
                {
                    i.AssetTagId,
                    i.AssetSerialNo,
                    i.ItemId,
                    i.AssetCost,
                    i.AssetDescription,
                    i.AssetContracts,
                    i.AssetPhotos,
                    i.AssetStatus.AssetStatusTitle,
                    DepreciationMethod = i.DepreciationMethod == null ? null : i.DepreciationMethod.DepreciationMethodTitle,
                    i.Item.ItemTitle,
                    i.Store.StoreTitle,
                    i.Vendor.VendorTitle,
                    i.Warranty
                });
                if (items.Count() == 0)
                    return Ok("No Matches Found");
                else
                    return Ok(new { items });
            }
            else
                return Ok("Enter Parcode To Search");
        }
        [HttpGet]
        public IActionResult Getassetinfobyid([FromQuery] int AssetId, [FromQuery] int TentantId)
        {
            if (AssetId != 0)
            {
                var items = _context.Assets.Where(c =>c.TenantId == TentantId && c.AssetId == AssetId
                ).Select(i => new
                {
                    i.AssetTagId,
                    i.AssetSerialNo,
                    i.ItemId,
                    i.AssetCost,
                    i.AssetDescription,
                    i.AssetContracts,
                    i.AssetPhotos,
                    i.AssetStatus.AssetStatusTitle,
                    DepreciationMethod = i.DepreciationMethod == null ? null : i.DepreciationMethod.DepreciationMethodTitle,
                    i.Item.ItemTitle,
                    i.Store.StoreTitle,
                    i.Vendor.VendorTitle,
                    i.Warranty
                });
                if (items.Count() == 0)
                    return Ok("Wrong AssetId");
                else
                    return Ok(new { items });
            }
            else
                return Ok("Enter AssetId");
        }
        [HttpGet]
        public IActionResult Getassetinfobyserial([FromQuery] string AssetSerialno, [FromQuery] int TentantId)
        {
            if (AssetSerialno != null)
            {
                var items = _context.Assets.Where(c =>c.TenantId== TentantId && c.AssetSerialNo == AssetSerialno
                ).Select(i => new
                {
                    i.AssetTagId,
                    i.AssetSerialNo,
                    i.ItemId,
                    i.AssetCost,
                    i.AssetDescription,
                    i.AssetContracts,
                    i.AssetPhotos,
                    i.AssetStatus.AssetStatusTitle,
                    DepreciationMethod = i.DepreciationMethod == null ? null : i.DepreciationMethod.DepreciationMethodTitle,
                    i.Item.ItemTitle,
                    i.Store.StoreTitle,
                    i.Vendor.VendorTitle,
                    i.Warranty,

                });
                if (items.Count() == 0)
                    return Ok("Wrong Serial Number");
                else
                    return Ok(new { items });
            }
            else
                return Ok("Enter Serial Number");
        }
        [HttpGet]
        public IActionResult Getassetinfobytag([FromQuery] string AssetTag, [FromQuery] int TentantId )
        {
            if (AssetTag != null)
            {
                var items = _context.Assets.Where(c =>c.TenantId==TentantId && c.AssetTagId == AssetTag
                ).Select(i => new
                {
                    i.AssetTagId,
                    i.AssetSerialNo,
                    i.ItemId,
                    i.AssetCost,
                    i.AssetDescription,
                    i.AssetContracts,
                    i.AssetPhotos,
                    i.AssetStatus.AssetStatusTitle,
                    DepreciationMethod = i.DepreciationMethod == null ? null : i.DepreciationMethod.DepreciationMethodTitle,
                    i.Item.ItemTitle,
                    i.Store.StoreTitle,
                    i.Vendor.VendorTitle,
                    i.Warranty
                });
                if (items.Count() == 0)
                    return Ok("Wrong AssetTag");
                else
                    return Ok(new { items });
            }
            else
                return Ok("Enter AssetTag");
        }
        [HttpGet]
        public IActionResult Getcheckedoutassets([FromQuery] string Search, [FromQuery] int TenantId )
        {
            if (Search != null)
            {
                var items = _context.AssetMovementDetails.Include(e=>e.Asset)
                   .Where(c =>c.Asset.TenantId==TenantId && c.Asset.AssetStatusId == 2 && (c.AssetMovement.Department.DepartmentTitle.Contains(Search) || c.AssetMovement.Employee.FullName.Contains(Search) || c.AssetMovement.Location.LocationTitle.Contains(Search)))
                   .Include(a => a.AssetMovement)
                .Select(i => new
                {
                    i.Asset.AssetTagId,
                    i.Asset.AssetSerialNo,
                    i.Asset.ItemId,
                    i.Asset.AssetCost,
                    i.Asset.AssetDescription,
                    i.Asset.AssetContracts,
                    i.Asset.AssetPhotos,
                    i.Asset.AssetStatus.AssetStatusTitle,
                    DepreciationMethod = i.Asset.DepreciationMethod == null ? null : i.Asset.DepreciationMethod.DepreciationMethodTitle,
                    i.Asset.Item.ItemTitle,
                    i.Asset.Store.StoreTitle,
                    i.Asset.Vendor.VendorTitle,
                    i.Asset.Warranty,
                    i.AssetMovement.Location.LocationTitle,
                    i.AssetMovement.Employee.FullName,
                    i.AssetMovement.Department.DepartmentTitle,
                    i.AssetMovement.AssetMovementDirection.AssetMovementDirectionTitle

                });

                if (items.Count() == 0)
                    return Ok("No Matches Found");
                else
                    return Ok(items);
            }
            else
                return Ok("Enter employee or location or department to search");
        }

        [HttpGet]
        public IActionResult GetAssetsByTenant([FromQuery] int TenantId)
        {
            var Asset = _context.Assets.Where(e => e.TenantId == TenantId).ToList();
            return Ok(Asset);
        }
        [HttpGet]
        public IActionResult GetLocationsByTenant([FromQuery] int TenantId)
        {
            var result = _context.Locations.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetDepartmentByTenant([FromQuery] int TenantId)
        {
            var result = _context.Departments.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetEmployeeByTenant([FromQuery] int TenantId)
        {
            var result = _context.Employees.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetCategoriesByTenant([FromQuery] int TenantId)
        {
            var result = _context.Categories.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetSubCategoriesByTenant([FromQuery] int TenantId)
        {
            var result = _context.SubCategories.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetItemsByTenant([FromQuery] int TenantId)
        {
            var result = _context.Items.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetStoreByTenant([FromQuery] int TenantId)
        {
            var result = _context.Stores.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetVendorByTenant([FromQuery] int TenantId)
        {
            var result = _context.Vendors.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetTechnicianByTenant([FromQuery] int TenantId)
        {
            var result = _context.Technicians.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetBrandByTenant([FromQuery] int TenantId)
        {
            var result = _context.Brands.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetContractByTenant([FromQuery] int TenantId)
        {
            var result = _context.Contracts.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetInsuranceByTenant([FromQuery] int TenantId)
        {
            var result = _context.Insurances.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetCustomerByTenant([FromQuery] int TenantId)
        {
            var result = _context.Customers.Where(e => e.TenantId == TenantId).ToList();
            return Ok(result);
        }
        [HttpGet]
        public IActionResult Getcheckedoutassetsbylocation([FromQuery] int LocationId, [FromQuery] int TentantId)
        {
            var checkedoutassets = new List<AssetVm>();
            if (LocationId != 0)
            {
                var movementsForLocation = _context.AssetMovements.Where(a =>a.Location.TenantId== TentantId && a.LocationId == LocationId && a.AssetMovementDirectionId == 1).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
                foreach (var item in movementsForLocation)
                {
                    foreach (var item2 in item.AssetMovementDetails)
                    {
                        if (item2.Asset.AssetStatusId == 2)
                        {
                            var lastassetmovement = _context.AssetMovementDetails.Where(a =>a.Asset.TenantId==item2.Asset.TenantId&& a.AssetId == item2.AssetId && a.AssetMovement.AssetMovementDirectionId == 1).Include(a => a.AssetMovement).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                            if (lastassetmovement.AssetMovement.LocationId == LocationId)
                            {
                                if (!checkedoutassets.Any(a => a.AssetId == item2.Asset.AssetId))
                                {
                                    checkedoutassets.Add(new AssetVm()
                                    {
                                        TenantId = item2.Asset.TenantId,
                                        PurchaseNo = item2.Asset.PurchaseNo,
                                        AssetCost = item2.Asset.AssetCost,
                                        AssetDescription = item2.Asset.AssetDescription,
                                        AssetId = item2.Asset.AssetId,
                                        AssetLife = item2.Asset.AssetLife,
                                        AssetPurchaseDate = item2.Asset.AssetPurchaseDate,
                                        AssetSerialNo = item2.Asset.AssetSerialNo,
                                        AssetStatusId = item2.Asset.AssetStatusId,
                                        AssetTagId = item2.Asset.AssetTagId,
                                        DateAcquired = item2.Asset.DateAcquired,
                                        DepreciableAsset = item2.Asset.DepreciableAsset,
                                        DepreciableCost = item2.Asset.DepreciableCost,
                                        DepreciationMethodId = item2.Asset.DepreciationMethodId,
                                        ItemId = item2.Asset.ItemId,
                                        Photo = item2.Asset.Photo,
                                        SalvageValue = item2.Asset.SalvageValue,
                                        StoreId = item2.Asset.StoreId,
                                        VendorId = item2.Asset.VendorId
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return Ok(checkedoutassets);
        }
        [HttpGet]
        public IActionResult GetcheckedoutassetsbyDepartment([FromQuery] int DepartmentId, [FromQuery] int TentantId)
        {
            var checkedoutassets = new List<AssetVm>();
            if (DepartmentId != 0)
            {
                var movementsForDepartment = _context.AssetMovements.Where(a => a.Department.TenantId == TentantId && a.DepartmentId == DepartmentId && a.AssetMovementDirectionId == 1 && a.EmpolyeeID == null).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
                foreach (var item in movementsForDepartment)
                {
                    foreach (var item2 in item.AssetMovementDetails)
                    {
                        if (item2.Asset.AssetStatusId == 2)
                        {
                            var lastassetmovement = _context.AssetMovementDetails.Where(a => a.Asset.TenantId == TentantId && a.AssetId == item2.AssetId && a.AssetMovement.AssetMovementDirectionId == 1).Include(a => a.AssetMovement).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                            if (lastassetmovement.AssetMovement.EmpolyeeID == null && lastassetmovement.AssetMovement.DepartmentId == DepartmentId)
                            {
                                if (!checkedoutassets.Any(a => a.AssetId == item2.Asset.AssetId))
                                {
                                    checkedoutassets.Add(new AssetVm()
                                    {
                                        TenantId = item2.Asset.TenantId,
                                        PurchaseNo = item2.Asset.PurchaseNo,
                                        AssetCost = item2.Asset.AssetCost,
                                        AssetDescription = item2.Asset.AssetDescription,
                                        AssetId = item2.Asset.AssetId,
                                        AssetLife = item2.Asset.AssetLife,
                                        AssetPurchaseDate = item2.Asset.AssetPurchaseDate,
                                        AssetSerialNo = item2.Asset.AssetSerialNo,
                                        AssetStatusId = item2.Asset.AssetStatusId,
                                        AssetTagId = item2.Asset.AssetTagId,
                                        DateAcquired = item2.Asset.DateAcquired,
                                        DepreciableAsset = item2.Asset.DepreciableAsset,
                                        DepreciableCost = item2.Asset.DepreciableCost,
                                        DepreciationMethodId = item2.Asset.DepreciationMethodId,
                                        ItemId = item2.Asset.ItemId,
                                        Photo = item2.Asset.Photo,
                                        SalvageValue = item2.Asset.SalvageValue,
                                        StoreId = item2.Asset.StoreId,
                                        VendorId = item2.Asset.VendorId
                                    });
                                }

                            }
                        }
                    }
                }
              
            }
            return Ok(checkedoutassets);
        }
        [HttpGet]
        public IActionResult GetcheckedoutassetsbyEmployee([FromQuery] int EmpolyeeID, [FromQuery] int TentantId)
        {
            var checkedoutassets = new List<AssetVm>();

            if (EmpolyeeID != 0)
            {
                var movementsForEmpolyee = _context.AssetMovements.Where(a =>a.Employee.TenantId== TentantId && a.EmpolyeeID == EmpolyeeID && a.AssetMovementDirectionId == 1).Include(a => a.AssetMovementDetails).ThenInclude(a => a.Asset);
                foreach (var item in movementsForEmpolyee)
                {
                    foreach (var item2 in item.AssetMovementDetails)
                    {
                        if (item2.Asset.AssetStatusId == 2)
                        {
                            var lastassetmovement = _context.AssetMovementDetails.Where(a =>a.Asset.TenantId == TentantId && a.AssetId == item2.AssetId && a.AssetMovement.AssetMovementDirectionId == 1).Include(a => a.AssetMovement).OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault();
                            if (lastassetmovement.AssetMovement.EmpolyeeID == EmpolyeeID)
                            {
                                if (!checkedoutassets.Any(a => a.AssetId == item2.Asset.AssetId))
                                {
                                    checkedoutassets.Add(new AssetVm()
                                    {
                                        TenantId = item2.Asset.TenantId,
                                        PurchaseNo = item2.Asset.PurchaseNo,
                                        AssetCost = item2.Asset.AssetCost,
                                        AssetDescription = item2.Asset.AssetDescription,
                                        AssetId = item2.Asset.AssetId,
                                        AssetLife = item2.Asset.AssetLife,
                                        AssetPurchaseDate = item2.Asset.AssetPurchaseDate,
                                        AssetSerialNo = item2.Asset.AssetSerialNo,
                                        AssetStatusId = item2.Asset.AssetStatusId,
                                        AssetTagId = item2.Asset.AssetTagId,
                                        DateAcquired = item2.Asset.DateAcquired,
                                        DepreciableAsset = item2.Asset.DepreciableAsset,
                                        DepreciableCost = item2.Asset.DepreciableCost,
                                        DepreciationMethodId = item2.Asset.DepreciationMethodId,
                                        ItemId = item2.Asset.ItemId,
                                        Photo = item2.Asset.Photo,
                                        SalvageValue = item2.Asset.SalvageValue,
                                        StoreId = item2.Asset.StoreId,
                                        VendorId = item2.Asset.VendorId
                                    });
                                }
                            }
                        }
                    }
                }

            }
            return Ok(checkedoutassets);
        }
          
        [HttpGet]
        public IActionResult GetTotalAssetCount([FromQuery] int TentantId)
        {
            try
            {
                var AllAssetscount = _context.Assets.Where(e=>e.TenantId==TentantId).Count();
                return Ok(new { AllAssetscount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }
        [HttpGet]
        public IActionResult GetTotalAssetCost([FromQuery] int TentantId)
        {
            try
            {
                var TotalAssetCost = _context.Assets.Where(e => e.TenantId == TentantId).Sum(a => a.AssetCost);
                return Ok(new { TotalAssetCost });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        [HttpGet]
        public IActionResult GetAvaliableAssetsCount([FromQuery] int TentantId)
        {
            try
            {
                var AvaliableAssets = _context.Assets.Where(a =>a.TenantId == TentantId && a.AssetStatusId == 1).Count();
                return Ok(new { AvaliableAssets });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        [HttpGet]
        public IActionResult GetActiveAssetsCount([FromQuery] int TentantId)
        {
            try
            {
                var ActiveAssets = _context.Assets.Where(a => a.TenantId == TentantId &&( a.AssetStatusId == 1 || a.AssetStatusId == 3 || a.AssetStatusId == 9 || a.AssetStatusId == 2)).Count();
                return Ok(new { ActiveAssets });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
           
       
    [HttpGet]
    public IActionResult GetAssetBrockenCount([FromQuery] int TentantId)
    {
        try
        {
            var AssetBrockenCount = _context.Assets.Where(a => a.TenantId == TentantId && a.AssetStatusId == 8).Count();
            return Ok(new { AssetBrockenCount });
        }

        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }
           
        [HttpGet]
        public IActionResult GetAssetBrockenCost([FromQuery] int TentantId)
        {
            try
            {
                var AssetBrockenCost = _context.Assets.Where(a => a.TenantId == TentantId && a.AssetStatusId == 8).Sum(a => a.AssetCost);
                return Ok(new { AssetBrockenCost });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
           
        }
        [HttpGet]
        public IActionResult GetSellAssetsCount([FromQuery] int TentantId)
        {
            try
            {
                var SellAssetsCount = _context.Assets.Where(a => a.TenantId == TentantId && a.AssetStatusId == 7).Count();
                return Ok(new { SellAssetsCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        [HttpGet]
        public IActionResult GetSellAssetCost([FromQuery] int TentantId)
        {
            var SellAssetCost = _context.AssetSellDetails.Where(e => e.Asset.TenantId == TentantId).Sum(a => a.SellAsset.SaleAmount);
            return Ok(new { SellAssetCost });
        }
        [HttpGet]
        public IActionResult GetAllDashBoardData([FromQuery] int TenantId)
        {
            try
            {

            var ActiveAssets = _context.Assets.Where(a => a.TenantId == TenantId && (a.AssetStatusId == 1 || a.AssetStatusId == 2 || a.AssetStatusId == 3 || a.AssetStatusId == 9)).Count();
            var TotalAssetCost = _context.Assets.Where(a => a.TenantId == TenantId).Sum(a => a.AssetCost);
            var AssetPurchaseCY = _context.Assets.Where(A => A.TenantId == TenantId&& A.AssetPurchaseDate.Date.Year == DateTime.Now.Year).Count();
            var AssetPurchaseCYCost = _context.Assets.Where(A => A.TenantId == TenantId&& A.AssetPurchaseDate.Date.Year == DateTime.Now.Year).Sum(a=>a.AssetCost);
            var AssetBrockenCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 8).Count();
            var AssetBrockenCost = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 8).Sum(a => a.AssetCost);
            var SellAssetsCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 7).Count();
                var SellAssetCost = _context.AssetSellDetails.Where(e => e.Asset.TenantId == TenantId && e.Asset.TenantId == TenantId).Sum(a => a.SellAsset.SaleAmount);
                var AssetCheckOutCount = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 2).Count();
            var AssetCheckOutCost = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 2).Sum(a => a.AssetCost);
                //var AssetLinkWarrantyCount = _context.AssetWarranties.Count();
               
                var AssetLinkWarrantyCount = (from W in
                   _context.AssetWarranties
                                          orderby W.AssetId
                                          select W.AssetId).Distinct().Count();

                //var AssetLinkWarrantyCost = _context.AssetWarranties.Distinct().Sum(a => a.Asset.AssetCost);
                var AssetIdWithWarranty = (from c in _context.AssetWarranties
                                           where c.Asset.TenantId == TenantId
                                           orderby c.AssetId
                                              select c.AssetId).Distinct();
                                                    
                double AssetLinkWarrantyCost = 0;
                foreach (var item in AssetIdWithWarranty)
                {

                    AssetLinkWarrantyCost += _context.Assets.Where(a =>a.TenantId == TenantId&& a.AssetId == item).Sum(a => a.AssetCost);
                }

                var AssetsUnderRepairCount = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 3).Count();
            var listmaxassetrepairId =
                 from a in _context.Assets
                 where a.TenantId== TenantId&& a.AssetStatusId == 3
                 from r in _context.AssetRepairs
                 from rd in _context.AssetRepairDetails
                 where a.AssetId == rd.AssetId && r.AssetRepairId == rd.AssetRepairId
                 group rd by rd.AssetId
                 into gr
                 select new
                 {
                     AMIDS = (from AMD in gr select AMD.AssetRepairId).Max()
                 };

            double AssetsUnderRepairCost = 0;
            foreach (var item in listmaxassetrepairId)
            {
                var costofmaxid = _context.AssetRepairs.Where(i =>i.AssetRepairId == item.AMIDS).Select(e => e.RepairCost).FirstOrDefault();
                AssetsUnderRepairCost += costofmaxid;
            }
            //insurance
            var totalinsurance = _context.Insurances.Where(a=>a.TenantId==TenantId).Count();
            //var AssetLinkInsuranceCount = _context.AssetsInsurances.Count();
                var AssetLinkInsuranceCount = (from IN in
              _context.AssetsInsurances
                                               orderby IN.AssetId
                                               select IN.AssetId).Distinct().Count();
                //var AssetLinkInsuranceCost = _context.AssetsInsurances.Sum(a => a.Asset.AssetCost);
                var AssetIdWithinsurance = (from c in _context.AssetsInsurances
                                           orderby c.AssetId
                                           select c.AssetId).Distinct();

                double AssetLinkInsuranceCost = 0;
                foreach (var item in AssetIdWithinsurance)
                {
                    AssetLinkInsuranceCost += _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetId == item).Sum(a => a.AssetCost);
                }


                var AvaliableAssetsCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 1).Count();
            var AvaliableAssetsCost = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 1).Sum(a=>a.AssetCost);

            var AssetsLeasedCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 6).Count();
            var listmaxassetLeasingId =
                 from a in _context.Assets
                 where a.TenantId == TenantId && a.AssetStatusId == 6
                 from r in _context.AssetLeasings
                 from rd in _context.AssetLeasingDetails
                 where a.AssetId == rd.AssetId && r.AssetLeasingId == rd.AssetLeasingId
                 group rd by rd.AssetId
                 into gr
                 select new
                 {
                     AMIDS = (from AMD in gr select AMD.AssetLeasingId).Max()
                 };
            double AssetsLeasingCost = 0;
            foreach (var item in listmaxassetLeasingId)
            {
                var costofmaxid = _context.AssetLeasings.Where(i =>i.AssetLeasingId == item.AMIDS).Select(e => e.LeasedCost).FirstOrDefault();
                AssetsLeasingCost += costofmaxid;
            }
            var AssetsDisposeCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 5).Count();
            var AssetsDisposeCost = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 5).Sum(a => a.AssetCost);
            var AssetsLostCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 4).Count();
            var AssetsLostCost = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 4).Sum(a => a.AssetCost);

            return Ok(new { ActiveAssets , TotalAssetCost , AssetPurchaseCY ,AssetPurchaseCYCost, AssetBrockenCount, AssetBrockenCost , SellAssetsCount, SellAssetCost, AssetCheckOutCount, AssetCheckOutCost, AssetLinkWarrantyCount, AssetLinkWarrantyCost,
            AssetsUnderRepairCount,AssetsUnderRepairCost,totalinsurance,AssetLinkInsuranceCount,AssetLinkInsuranceCost,
                AvaliableAssetsCount,AvaliableAssetsCost,AssetsLeasedCount,AssetsLeasingCost,AssetsDisposeCount,AssetsDisposeCost,AssetsLostCount,AssetsLostCost
            });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAssetsUnderRepairCount([FromQuery] int TenantId)
        {
            var AssetsUnderRepairCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 3).Count();
            return Ok(new { AssetsUnderRepairCount });
        }
        [HttpGet]
        public IActionResult GetAssetsUnderRepairCost([FromQuery] int TenantId)
        {
            var listmaxassetrepairId =
                 from a in _context.Assets
                 where a.TenantId == TenantId && a.AssetStatusId == 3
                 from r in _context.AssetRepairs
                 from rd in _context.AssetRepairDetails
                 where a.AssetId == rd.AssetId && r.AssetRepairId == rd.AssetRepairId
                 group rd by rd.AssetId
                 into gr
                 select new
                 {
                     AMIDS = (from AMD in gr select AMD.AssetRepairId).Max()
                 };

            double AssetsUnderRepairCost = 0;
            foreach (var item in listmaxassetrepairId)
            {
                var costofmaxid = _context.AssetRepairs.Where(i => i.AssetRepairId == item.AMIDS).Select(e => e.RepairCost).FirstOrDefault();
                AssetsUnderRepairCost += costofmaxid;
            }
            return Ok(new { AssetsUnderRepairCost });
        }
        [HttpGet]
        public IActionResult GetAssetsLeasedCount([FromQuery] int TenantId)
        {
            var AssetsLeasedCount = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 6).Count();
            return Ok(new { AssetsLeasedCount });
        }
        [HttpGet]
        public IActionResult GetLeasedAssetsCost([FromQuery] int TenantId)
        {
            var listmaxassetLeasingId =
                 from a in _context.Assets
                 where a.TenantId == TenantId && a.AssetStatusId == 6
                 from r in _context.AssetLeasings
                 from rd in _context.AssetLeasingDetails
                 where a.AssetId == rd.AssetId && r.AssetLeasingId == rd.AssetLeasingId
                 group rd by rd.AssetId
                 into gr
                 select new
                 {
                     AMIDS = (from AMD in gr select AMD.AssetLeasingId).Max()
                 };
            double AssetsLeasingCost = 0;
            foreach (var item in listmaxassetLeasingId)
            {
                var costofmaxid = _context.AssetLeasings.Where(i => i.AssetLeasingId == item.AMIDS).Select(e => e.LeasedCost).FirstOrDefault();
                AssetsLeasingCost += costofmaxid;
            }
            return Ok(new { AssetsLeasingCost });
        }
        [HttpGet]
        public IActionResult GetAssetsLostCount([FromQuery] int TenantId)
        {
            var AssetsLostCount = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 4).Count();
            return Ok(new { AssetsLostCount });
        }
        [HttpGet]
        public IActionResult GetAssetsLostCost([FromQuery] int TenantId)
        {
            var AssetsLostCost = _context.Assets.Where(a =>a.TenantId == TenantId && a.AssetStatusId == 4).Sum(a => a.AssetCost);
            return Ok(new { AssetsLostCost });
        }
        [HttpGet]
        public IActionResult GetAssetsDisposeCount([FromQuery] int TenantId)
        {
            var AssetsDisposeCount = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 5).Count();
            return Ok(new { AssetsDisposeCount });
        }
        [HttpGet]
        public IActionResult GetAssetsDisposeCost([FromQuery] int TenantId)
        {
            var AssetsDisposeCost = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 5).Sum(a => a.AssetCost);
            return Ok(new { AssetsDisposeCost });
        }
        [HttpGet]
        public IActionResult GetAssetsMaintCount([FromQuery] int TenantId)
        {
            var AssetsMaintCount = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 9).Count();
            return Ok(new { AssetsMaintCount });
        }
        [HttpGet]
        public IActionResult GetAssetsMaintCost([FromQuery] int TenantId)
        {
            var AssetsMaintCost = _context.AssetMaintainances.Where(a => a.Asset.TenantId == TenantId).Sum(a => a.AssetMaintainanceRepairesCost);
            return Ok(new { AssetsMaintCost });
        }
        [HttpGet]
        public IActionResult GetAssetCheckOutCount([FromQuery] int TenantId)
        {
            var AssetCheckOutCount = _context.Assets.Where(a => a.TenantId == TenantId&& a.AssetStatusId == 2).Count();
            return Ok(new { AssetCheckOutCount });
        }
        [HttpGet]
        public IActionResult GetAssetCheckOutCost([FromQuery] int TenantId)
        {
            var AssetCheckOutCost = _context.Assets.Where(a => a.TenantId == TenantId && a.AssetStatusId == 2).Sum(a => a.AssetCost);
            return Ok(new { AssetCheckOutCost });
        }
        [HttpGet]
        public IActionResult GetAssetLinkInsuranceCountAndCost([FromQuery] int TenantId)
        {
            var AssetLinkInsuranceCount = _context.AssetsInsurances.Where(a => a.Asset.TenantId == TenantId).Count();
            var AssetLinkInsuranceCost = _context.AssetsInsurances.Where(a => a.Asset.TenantId == TenantId).Sum(a => a.Asset.AssetCost);
            return Ok(new { Count = AssetLinkInsuranceCount, Cost = AssetLinkInsuranceCost });
        }
        [HttpGet]
        public IActionResult GetAssetWithoutInsuranceCountAndCost([FromQuery] int TenantId)
        {
            var AssetIdWithoutInsurance = from c in _context.Assets
                                          where !(from o in _context.AssetsInsurances
                                                  select o.AssetId)
                                                 .Contains(c.AssetId)&&c.TenantId== TenantId
                                          select c;
            double cost = 0;
            foreach (var item in AssetIdWithoutInsurance)
            {
                cost += item.AssetCost;
            }
            var AssetWithoutInsuranceCount = AssetIdWithoutInsurance.Count();
            return Ok(new { Count = AssetWithoutInsuranceCount, Cost = cost });
        }

        [HttpGet]
        public IActionResult GetAssetLinkWarrantyCountAndCost([FromQuery] int TenantId)
        {
            var AssetLinkWarrantyCount = _context.AssetWarranties.Where(a => a.Asset.TenantId == TenantId).Count();
            var AssetLinkWarrantyCost = _context.AssetWarranties.Where(a => a.Asset.TenantId == TenantId).Sum(a => a.Asset.AssetCost);
            return Ok(new { Count = AssetLinkWarrantyCount, Cost = AssetLinkWarrantyCost });
        }
        [HttpGet]
        public IActionResult GetAssetWithoutWarrantyCountAndCost([FromQuery] int TenantId)
        {
            var AssetIdWithoutWarranty = from c in _context.Assets
                                         where !(from o in _context.AssetWarranties
                                                 select o.AssetId)
                                                .Contains(c.AssetId)&& c.TenantId == TenantId
                                         select c;
            double AssetWithoutInsuranceCost = 0;
            foreach (var item in AssetIdWithoutWarranty)
            {
                AssetWithoutInsuranceCost += item.AssetCost;
            }
            var AssetWithoutInsuranceCount = AssetIdWithoutWarranty.Count();
            return Ok(new { Count = AssetWithoutInsuranceCount, Cost = AssetWithoutInsuranceCost });
        }



        [HttpGet]
        public IActionResult GetAssetLinkInsuranceCost([FromQuery] int TenantId)
        {
            var AssetLinkInsuranceCost = _context.AssetsInsurances.Include(e => e.Asset).Where(a => a.Asset.TenantId == TenantId).Select(e => e.Asset.AssetCost).Sum();
            return Ok(new { AssetLinkInsuranceCost });
        }
        [HttpGet]
        public IActionResult GetAssetLinkContractCount([FromQuery] int TenantId)
        {
            var AssetLinkContractCount = _context.AssetContracts.Where(a => a.Asset.TenantId == TenantId).Count();
            return Ok(new { AssetLinkContractCount });
        }
        [HttpGet]
        public IActionResult GetAssetsPurchaseCurrentYear([FromQuery] int TenantId)
        {
            var AssetPurchaseCY = _context.Assets.Where(A => A.TenantId == TenantId && A.AssetPurchaseDate.Date.Year == DateTime.Now.Year).Count();
            return Ok(new { AssetPurchaseCY });
        }
        [HttpGet]
        public IActionResult GetAssetsPurchaseCurrentYearCost([FromQuery] int TenantId)
        {
            var AssetPurchaseCostCY = _context.Assets.Where(A => A.TenantId == TenantId && A.AssetPurchaseDate.Date.Year == DateTime.Now.Year).Sum(a => a.AssetCost);
            return Ok(new { AssetPurchaseCostCY });
        }
        //start Alerts
        [HttpGet]
        public IActionResult GetAlertsCounts([FromQuery] int TenantId)
        {
            try
            {
                var ContractsExpiringCount = _context.Contracts.Where(c => c.TenantId == TenantId && c.EndDate.Date < DateTime.Now.Date).Count();
                var InsurancesExpiringCount = _context.Insurances.Where(c => c.TenantId == TenantId && c.EndDate.Date < DateTime.Now.Date).Count();
                var MaintenancedueCount = _context.AssetMaintainances.Include(e => e.Asset).Where(c => c.Asset.TenantId == TenantId && c.ScheduleDate.Date == DateTime.Now.Date && c.MaintainanceStatus.MaintainanceStatusId == 1 && c.Asset.AssetStatusId == 9).Count();
                var MaintenanceoverdueCount = _context.AssetMaintainances.Include(e => e.Asset).Where(c => c.Asset.TenantId == TenantId && c.ScheduleDate.Date < DateTime.Now.Date && c.MaintainanceStatus.MaintainanceStatusId == 1 && c.Asset.AssetStatusId == 9).Count();
                var WarrantiesExpiringCount = _context.AssetWarranties.Where(c => c.Asset.TenantId == TenantId && c.ExpirationDate.Date < DateTime.Now.Date).Count();
                return Ok(new { ContractsExpiringCount, InsurancesExpiringCount, MaintenancedueCount, MaintenanceoverdueCount, WarrantiesExpiringCount });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetContractsExpiringCount([FromQuery] int TenantId)
        {
            try
            {
                var ContractsExpiringCount = _context.Contracts.Where(c => c.TenantId == TenantId && c.EndDate.Date < DateTime.Now.Date).Count();
                return Ok(new { ContractsExpiringCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetContractsExpiringList([FromQuery] int TenantId)
        {
            try
            {

                var contracts = _context.Contracts.Where(c =>c.TenantId== TenantId && c.EndDate.Date < DateTime.Now.Date).Select(i => new
                {
                    i.Title,
                    i.Description,
                    i.ContractNo,
                    i.Cost,
                    i.StartDate,
                    i.EndDate,
                    i.Vendor.VendorTitle
                });
                return Ok(contracts);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetInsurancesExpiringCount([FromQuery] int TenantId)
        {
            try
            {
                var InsurancesExpiringCount = _context.Insurances.Where(c => c.TenantId == TenantId && c.EndDate.Date < DateTime.Now.Date).Count();
                return Ok(new { InsurancesExpiringCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetInsurancesExpiringList([FromQuery] int TenantId)
        {
            try
            {


                var insurances = _context.Insurances.Where(c => c.TenantId == TenantId && c.EndDate.Date < DateTime.Now.Date).Select(i => new
                {
                    i.Title,
                    i.ContactPerson,
                    i.InsuranceCompany,
                    i.StartDate,
                    i.EndDate,
                    i.PolicyNo

                });
                return Ok(insurances);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetMaintainanceDueCount([FromQuery] int TenantId)
        {
            try
            {
                var MaintainanceDueCount = _context.AssetMaintainances.Where(c => c.Asset.TenantId == TenantId && c.ScheduleDate.Date == DateTime.Now.Date && c.MaintainanceStatus.MaintainanceStatusId == 1).Count();
                return Ok(new { MaintainanceDueCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetMaintenanceDueList([FromQuery] int TenantId)
        {
            try
            {


                var Maintainances = _context.AssetMaintainances.Where(c => c.Asset.TenantId == TenantId && c.ScheduleDate.Date == DateTime.Now.Date && c.MaintainanceStatus.MaintainanceStatusId == 1).Include(i => i.Asset).Include(i => i.Technician).Select(i => new
                {
                    i.AssetMaintainanceId,
                    i.AssetMaintainanceTitle,
                    i.AssetMaintainanceRepairesCost,
                    i.ScheduleDate,
                    i.AssetMaintainanceDetails,
                    i.Technician,
                    i.Asset.AssetTagId,
                    i.Asset.Photo,
                    i.Asset.AssetCost,
                    i.Asset.AssetSerialNo

                });
                return Ok(Maintainances);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetMaintainanceOverDueCount([FromQuery] int TenantId)
        {
            try
            {
                var MaintainanceOverDueCount = _context.AssetMaintainances.Where(c => c.Asset.TenantId == TenantId && c.ScheduleDate.Date < DateTime.Now.Date && c.MaintainanceStatus.MaintainanceStatusId == 1).Count();
                return Ok(new { MaintainanceOverDueCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetMaintenanceoverDueList([FromQuery] int TenantId)
        {
            try
            {

                var Maintainances = _context.AssetMaintainances.Where(c => c.Asset.TenantId == TenantId && c.ScheduleDate.Date < DateTime.Now.Date && c.MaintainanceStatus.MaintainanceStatusId == 1).Include(i => i.Asset).Include(i => i.Technician).Select(i => new
                {
                    i.AssetMaintainanceId,
                    i.AssetMaintainanceTitle,
                    i.AssetMaintainanceRepairesCost,
                    i.ScheduleDate,
                    i.AssetMaintainanceDetails,
                    i.Technician,
                    i.Asset.AssetTagId,
                    i.Asset.Photo,
                    i.Asset.AssetCost,
                    i.Asset.AssetSerialNo
                });
                return Ok(Maintainances);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetWarrantiesExpiringCount([FromQuery] int TenantId)
        {
            try
            {
                var WarrantiesExpiringCount = _context.AssetWarranties.Where(c => c.Asset.TenantId == TenantId && c.ExpirationDate.Date < DateTime.Now.Date).Count();
                return Ok(new { WarrantiesExpiringCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetWarrantiesExpiringList([FromQuery] int TenantId)
        {
            try
            {


                var warranty = _context.AssetWarranties.Where(c => c.Asset.TenantId == TenantId && c.ExpirationDate.Date < DateTime.Now.Date).Include(i => i.Asset).Select(i => new
                {
                    i.ExpirationDate,
                    i.Length,
                    i.Notes,
                    i.Asset.AssetCost,
                    i.Asset.AssetSerialNo,
                    i.Asset.Photo,
                    i.Asset.AssetTagId
                });
                return Ok(warranty);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAssetsPastDueCount([FromQuery] int TenantId)
        {
            try
            {
                var AssetsCheckOut = _context.Assets.Include(i => i.AssetMovementDetails).ThenInclude(i => i.AssetMovement).Where(c => c.TenantId == TenantId && c.AssetStatus.AssetStatusId == 2 && c.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.DueDate < DateTime.Now.Date).Count();
                return Ok(new { AssetsCheckOut });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        public IActionResult GetAssetsPastDueList([FromQuery] int TenantId)
        {
            try
            {
                var AssetsCheckOut = _context.Assets.Include(i => i.AssetMovementDetails).ThenInclude(i => i.AssetMovement).Where(c => c.TenantId == TenantId && c.AssetStatus.AssetStatusId == 2 && c.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.DueDate < DateTime.Now.Date).Select(i => new AssetProject.ReportModels.AssetReportsModel
                {
                    AssetID = i.AssetId,
                    AssetCost = i.AssetCost,
                    AssetSerialNo = i.AssetSerialNo,
                    AssetTagId = i.AssetTagId,
                    Photo = i.Photo,
                    TransactionDate = i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.TransactionDate,
                    DueDate = i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.DueDate,
                    LocationTL = _context.Locations.Where(a =>a.TenantId==TenantId&& a.LocationId == i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.LocationId).FirstOrDefault().LocationTitle,
                    DepartmentTL = _context.Departments.Where(a =>a.TenantId==TenantId && a.DepartmentId == i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.DepartmentId).FirstOrDefault().DepartmentTitle,
                    EmployeeFullName = i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.EmpolyeeID == null ? null : _context.Employees.Where(a => a.ID == i.AssetMovementDetails.OrderByDescending(e => e.AssetMovementDetailsId).FirstOrDefault().AssetMovement.EmpolyeeID).FirstOrDefault().FullName,

                });

                return Ok(AssetsCheckOut);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetLeasesExpiringCount([FromQuery] int TenantId)
        {
            try
            {
                var LeasingCount = _context.Assets.Include(i => i.AssetLeasingDetails).ThenInclude(i => i.AssetLeasing).Where(c => c.TenantId == TenantId && c.AssetStatus.AssetStatusId == 6 && c.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.EndDate.Date < DateTime.Now.Date).Count();
                return Ok(new { LeasingCount });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetLeasesExpiringList([FromQuery] int TenantId)
        {
            try
            {
                var leasing = _context.Assets.Include(i => i.AssetLeasingDetails).ThenInclude(i => i.AssetLeasing).Where(c => c.TenantId == TenantId && c.AssetStatus.AssetStatusId == 6 && c.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.EndDate.Date < DateTime.Now.Date).Select(i => new AssetProject.ReportModels.LeasingModel
                {
                    AssetId = i.AssetId,
                    AssetCost = i.AssetCost,
                    AssetSerialNo = i.AssetSerialNo,
                    AssetTagId = i.AssetTagId,
                    photo = i.Photo,

                    LeasingEndDate = i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.EndDate,
                    LeasingStartDate = i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.StartDate,
                    LeasingCost = i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.LeasedCost,
                    CustomerTL = _context.Customers.Where(a => a.TenantId == TenantId && a.CustomerId == i.AssetLeasingDetails.OrderByDescending(e => e.AssetLeasingDetailsId).FirstOrDefault().AssetLeasing.CustomerId).FirstOrDefault().FullName,


                });
                return Ok(leasing);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        //end Alerts

        [HttpGet]
        public IActionResult GetCheckOutList([FromQuery] int TenantId)
        {
            var CheckOutList = _context.Assets.Where(c => c.TenantId == TenantId && c.AssetStatusId == 2);
            return Ok(new { CheckOutList });
        }
        [HttpGet]
        public IActionResult GetCheckInList([FromQuery] int TenantId)
        {
            var CheckInList = _context.Assets.Where(c => c.TenantId == TenantId && c.AssetStatusId == 1);
            return Ok(new { CheckInList });
        }
        [HttpGet]
        public IActionResult GetUnderRepairList([FromQuery] int TenantId)
        {
            var UnderRepairList = _context.Assets.Where(c => c.TenantId == TenantId && c.AssetStatusId == 3);
            return Ok(new { UnderRepairList });
        }
        [HttpGet]
        public IActionResult GetAllAssets([FromQuery] int TenantId)
        {
            var AllAssets = _context.Assets.Where(c => c.TenantId == TenantId).ToList();
            return Ok(new { AllAssets });
        }
        [HttpGet]
        public IActionResult GetAssetDetails(int? AssetId, int TenantId)
        {
            if (AssetId != null && AssetId != 0 && AssetId != 0)
            {
                var ischeckoutorin = _context.Assets.Where(c => c.TenantId == TenantId).FirstOrDefault(e=>e.AssetId==AssetId);
                if (ischeckoutorin != null)
                {
                    if (ischeckoutorin.AssetStatusId == 2 || ischeckoutorin.AssetStatusId == 1)
                    {
                        var MaxMovementId =
                     (from r in _context.AssetMovements
                      from rd in _context.AssetMovementDetails
                      where ischeckoutorin .TenantId== TenantId && AssetId == rd.AssetId && r.AssetMovementId == rd.AssetMovementId
                      orderby rd.AssetMovementDetailsId descending
                      select rd.AssetMovementId
                      ).FirstOrDefault();
                        var Assetdetails = _context.Assets.Where(a =>a.TenantId==TenantId&& a.AssetId == AssetId && (a.AssetStatusId == 1 || a.AssetStatusId == 2)).Select(i => new
                        {
                            i.AssetTagId,
                            i.AssetSerialNo,
                            i.AssetPurchaseDate,
                            i.ItemId,
                            i.AssetCost,
                            i.AssetDescription,
                            i.AssetStatus.AssetStatusTitle,
                            DepricableCost = i.DepreciableAsset == false ? 0 : i.DepreciableCost,
                            SalvageValue = i.DepreciableAsset == false ? 0 : i.SalvageValue,
                            DateAcquired = i.DepreciableAsset == false ? null : i.DateAcquired,
                            AssetLife = i.DepreciableAsset == false ? 0 : i.AssetLife,
                            DepreciationMethod = i.DepreciableAsset == false ? null : i.DepreciationMethod.DepreciationMethodTitle,
                            i.Item.ItemTitle,
                            Store = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().Store.StoreTitle,
                            i.Vendor.VendorTitle,
                            i.Item.Brand.BrandTitle,
                            i.Vendor.Website,
                            i.Item.Category.CategoryTIAR,
                            employee = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().EmpolyeeID == null ? null : _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().Employee.FullName,
                            department = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().DepartmentId == null ? null : _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().Department.DepartmentTitle,
                            Location = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().LocationId == null ? null : _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().Location.LocationTitle,
                            TransactionDate = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault() == null ? null : _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().TransactionDate,
                            DueDate = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault() == null ? null : _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().DueDate,
                            Notes = _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault() == null ? null : _context.AssetMovements.Where(e => e.AssetMovementId == MaxMovementId).FirstOrDefault().Remarks
                        });
                        return Ok(Assetdetails);
                    }
                    else
                    {
                        var Assetdetails = _context.Assets.Where(a =>a.TenantId==TenantId&& a.AssetId == AssetId).Select(i => new
                        {
                            i.AssetTagId,
                            i.AssetSerialNo,
                            i.AssetPurchaseDate,
                            i.ItemId,
                            i.AssetCost,
                            i.AssetDescription,
                            i.AssetStatus.AssetStatusTitle,
                            DepricableCost = i.DepreciableAsset == false ? 0 : i.DepreciableCost,
                            SalvageValue = i.DepreciableAsset == false ? 0 : i.SalvageValue,
                            DateAcquired = i.DepreciableAsset == false ? null : i.DateAcquired,
                            AssetLife = i.DepreciableAsset == false ? 0 : i.AssetLife,
                            DepreciationMethod = i.DepreciableAsset == false ? null : i.DepreciationMethod.DepreciationMethodTitle,
                            i.Item.ItemTitle,
                            i.Store.StoreTitle,
                            i.Vendor.VendorTitle,
                            i.Item.Brand.BrandTitle,
                            i.Vendor.Website,
                            i.Item.Category.CategoryTIAR,
                        });
                        return Ok(Assetdetails);
                    }
                }
                return BadRequest($"Asset {AssetId} Not Found..");
            }
            return BadRequest("Enter AssetId..");
        }
        [HttpGet]
        public IActionResult GetAssetHistory(int? AssetId, [FromQuery] int TenantId)
        {
            if (AssetId != null && AssetId != 0)
            {
                try
                {
                    var assetlogs = _context.AssetLogs.Where(e =>e.Asset.TenantId==TenantId&& e.AssetId == AssetId).Select(i => new
                    {
                        i.ActionDate,
                        i.Remark
                    });
                    return Ok(assetlogs);
                }
                catch (Exception)
                {

                    return BadRequest("Server Error..");
                }

            }
            return BadRequest("Enter Asset Id");
        }

        [HttpPost]
        public IActionResult PostAddAssetPhoto(IFormFile file, AssetPhotos photos)
        {
            if (file != null)
            {
                string folder = "Images/AssetPhotos/";
                photos.PhotoUrl = UploadImage(folder, file);
                if (photos.AssetId != 0)
                {
                    _context.AssetPhotos.Add(photos);
                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 5,
                        AssetId = photos.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($" Description : {photos.Remarks} ")
                    };
                    _context.AssetLogs.Add(assetLog);
                    _context.SaveChanges();
                    return Ok("Image Uploaded Successfully..");
                }
                return BadRequest("Please Enter Asset Id.. ");
            }
            return BadRequest("Please Choose Image.. ");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
        [HttpGet]
        public IActionResult GetAssetPhotos(int? AssetId, [FromQuery] int TenantId)
        {

            if (AssetId != null && AssetId != 0)
            {

                var ListPhotos = _context.AssetPhotos.Where(a =>a.Asset.TenantId==TenantId && a.AssetId == AssetId);
                if (ListPhotos == null)
                {
                    return BadRequest("Asset Not Found..");
                }
                return Ok(ListPhotos);

            }
            return BadRequest("Enter Asset Id..");

        }
        [HttpDelete]
        public IActionResult DeleteAssetPhoto(int? AssetId, int? AssetPhotoId)
        {
            if (AssetId != null && AssetId != 0)
            {
                if (AssetPhotoId != null)
                {
                    var Result = _context.AssetPhotos.Where(a => a.AssetId == AssetId && a.AssetPhotoId == AssetPhotoId).FirstOrDefault();
                    if (Result != null)
                    {
                        try
                        {
                            _context.AssetPhotos.Remove(Result);
                            AssetLog assetLog = new AssetLog()
                            {
                                ActionLogId = 9,
                                AssetId = AssetId.Value,
                                ActionDate = DateTime.Now,
                            };
                            _context.AssetLogs.Add(assetLog);
                            _context.SaveChanges();
                            if (Result.PhotoUrl != null)
                            {
                                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, Result.PhotoUrl);
                                if (System.IO.File.Exists(ImagePath))
                                {
                                    System.IO.File.Delete(ImagePath);
                                }
                            }
                            return Ok("Photo Deleted Successfully..");
                        }
                        catch (Exception)
                        {

                            return BadRequest("Server Error..");
                        }

                    }
                    return BadRequest("photo Not Found..");

                }
                return BadRequest("Enter Asset Photo Id..");
            }
            return BadRequest("Enter Asset Id..");
        }
        [HttpGet]
        public IActionResult GetAssetDocuments(int? AssetId, [FromQuery] int TenantId)
        {
            if (AssetId != null && AssetId != 0)
            {
                try
                {
                    var assetdocuments = _context.assetDocuments.Where(e =>e.Asset.TenantId==TenantId&& e.AssetId == AssetId).Select(i => new
                    {
                        i.DocumentName,
                        i.DocumentType,
                        i.Description
                    });
                    return Ok(assetdocuments);
                }
                catch (Exception)
                {

                    return BadRequest("Server Error..");
                }
            }
            return BadRequest("Enter Correct Asset Id..");
        }

        [HttpPost]
        public IActionResult PostAddAssetDocument(AssetDocument instance, IFormFile file)
        {

            if (file != null)
            {
                string folder = "Documents/AssetDocuments/";
                instance.DocumentType = UploadImage(folder, file);
                if (instance.AssetId != 0)
                {
                    if (instance.DocumentName != null)
                    {
                        try
                        {
                            _context.assetDocuments.Add(instance);
                            AssetLog assetLog = new AssetLog()
                            {
                                ActionLogId = 4,
                                AssetId = instance.AssetId,
                                ActionDate = DateTime.Now,
                                Remark = string.Format($"Document Name : {instance.DocumentName} ")
                            };
                            _context.AssetLogs.Add(assetLog);
                            _context.SaveChanges();
                            return Ok("Document Uploaded Successfully..");
                        }
                        catch (Exception)
                        {

                            return BadRequest("Server Error...");
                        }

                    }
                    return BadRequest("Please Enter Document Name.. ");

                }
                return BadRequest("Please Enter Asset Id.. ");
            }
            return BadRequest("Please Choose Document.. ");
        }
        [HttpDelete]
        public IActionResult DeattachAssetDocument(int? AssetDocumentID)
        {

            if (AssetDocumentID != 0 && AssetDocumentID != null)
            {

                AssetDocument _assetDocument = _context.assetDocuments.FirstOrDefault(e=>e.AssetDocumentId==AssetDocumentID);
                if (_assetDocument != null)
                {
                    string AssetDocName = _assetDocument.DocumentName;
                    try
                    {
                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 8,
                            AssetId = _assetDocument.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"Dettached Asset Document With Document Name : {AssetDocName} ")
                        };
                        _context.AssetLogs.Add(assetLog);
                        _context.assetDocuments.Remove(_assetDocument);
                        _context.SaveChanges();
                        return Ok("Asset Document Deleted Succeffully");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                else
                    return BadRequest("Document not Found..");
            }
            return BadRequest("Enter Asset Document Id..");
        }
        [HttpGet]
        public IActionResult GetAllContracts([FromQuery] int TenantId)
        {

            try
            {
                var Allcontracts = _context.Contracts.Where(e => e.TenantId == TenantId).Select(i => new
                {
                    i.Title,
                    i.Description,
                    i.ContractNo,
                    i.Cost,
                    i.StartDate,
                    i.EndDate,
                    i.Vendor.VendorTitle
                });
                return Ok(Allcontracts);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAllActionType()
        {

            try
            {
                var AllActionType = _context.ActionTypes.Select(i => new
                {
                    i.ActionTypeTitle,
                    i.ActionTypeId
                });
                return Ok(AllActionType);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAssetContractById(int? AssetId, [FromQuery] int TenantId)
        {

            if (AssetId != null && AssetId != 0)
            {
                try
                {
                    var Allcontracts = _context.AssetContracts.Where(e => e.Asset.TenantId == TenantId&& e.AssetId == AssetId).Select(i => new
                    {
                        i.Contract.Description,
                        i.Contract.ContractNo,
                        i.Contract.Cost,
                        i.Contract.StartDate,
                        i.Contract.EndDate,
                        i.Contract.Title,
                        i.Contract.Vendor.VendorTitle,
                    });
                    return Ok(Allcontracts);

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Enter Asset Id.. ");
        }
        [HttpPost]
        public IActionResult PostLinkAssetContract(AssetContract assetcontract)
        {
            if (assetcontract.AssetId != 0)
            {

                if (assetcontract.ContractId != null)
                {
                    try
                    {
                        var Assetcont = new AssetContract { AssetId = assetcontract.AssetId, ContractId = assetcontract.ContractId };
                        _context.AssetContracts.Add(Assetcont);
                        string ContractTitle = "Contract Title : ";
                        string ContractSDate = "Contract Start Date : ";
                        string ContractEDate = "Contract End Date : ";
                        Contract SelectedContract = _context.Contracts.Find(assetcontract.ContractId);
                        string ContractStartDate = SelectedContract.StartDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                        string ContractEndDate = SelectedContract.EndDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 2,
                            AssetId = assetcontract.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"{ContractTitle}{SelectedContract.Title} , {ContractSDate}{ContractStartDate} and {ContractEDate}{ContractEndDate}")
                        };
                        _context.AssetLogs.Add(assetLog);
                        _context.SaveChanges();
                        return Ok("Link Asset Contract Successfully..");
                    }
                    catch (Exception e)
                    {

                        return BadRequest(e.Message);
                    }
                }
                return BadRequest("Enter Contract ID..");
            }
            return BadRequest("Enter Asset ID..");
        }
        [HttpDelete]
        public IActionResult DeleteAssetContract(AssetContract assetContract)
        {
            if (assetContract.AssetId != 0)
            {
                if (assetContract.ContractId != null && assetContract.ContractId != 0)
                {
                    AssetContract _assetContract = _context.AssetContracts.Where(e => e.ContractId == assetContract.ContractId && e.AssetId == assetContract.AssetId).FirstOrDefault();
                    if (_assetContract == null)
                    {
                        return BadRequest("Asset Contract Not Found..");
                    }
                    try
                    {
                        _context.AssetContracts.Remove(_assetContract);
                        Contract contract = _context.Contracts.Find(assetContract.ContractId);

                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 6,
                            AssetId = assetContract.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"Dettached Asset Contract With Contract Name : {contract.Title} and Contract Number : {contract.ContractNo}")
                        };
                        _context.AssetLogs.Add(assetLog);
                        _context.SaveChanges();
                        return Ok("Asset Contract Dettached Succeffully");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                return BadRequest("Enter Contract Id..");
            }
            return BadRequest("Enter Asset Id..");
        }
        [HttpGet]
        public IActionResult GetAllInsurances([FromQuery] int TenantId)
        {
            try
            {
                var insurances = _context.Insurances.Where(e => e.TenantId == TenantId).Select(i => new
                {
                    i.Title,
                    i.Description,
                    i.InsuranceCompany,
                    i.ContactPerson,
                    i.PolicyNo,
                    i.Phone,
                    i.StartDate,
                    i.EndDate,
                    i.Deductible,
                    i.Permium,
                    i.IsActive
                });
                return Ok(insurances);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAssetInsuranceById(int? AssetId, [FromQuery] int TenantId)
        {
            if (AssetId != null && AssetId != 0)
            {
                try
                {
                    var assetsinsurances = _context.AssetsInsurances.Where(e => e.Asset.TenantId == TenantId && e.AssetId == AssetId).Select(i => new
                    {
                        i.Insurance.Title,
                        i.Insurance.Description,
                        i.Insurance.InsuranceCompany,
                        i.Insurance.ContactPerson,
                        i.Insurance.PolicyNo,
                        i.Insurance.Phone,
                        i.Insurance.StartDate,
                        i.Insurance.EndDate,
                        i.Insurance.Deductible,
                        i.Insurance.Permium,
                        i.Insurance.IsActive
                    });
                    return Ok(assetsinsurances);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Enter Asset Id..");
        }
        [HttpPost]
        public IActionResult postAddAssetInsurance(AssetsInsurance assetsInsurance)
        {
            if (assetsInsurance.InsuranceId != null && assetsInsurance.InsuranceId != 0)
            {
                if (assetsInsurance.AssetId != 0)
                {
                    try
                    {
                        AssetsInsurance AssetIns = new AssetsInsurance { AssetId = assetsInsurance.AssetId, InsuranceId = assetsInsurance.InsuranceId };
                        _context.AssetsInsurances.Add(AssetIns);
                        string InsuranceTitle = "Insurance Title : ";
                        string InsuranceCompany = "Insurance Company : ";
                        Insurance SelectedInsurance = _context.Insurances.Find(assetsInsurance.InsuranceId);
                        string InsuranceTit = SelectedInsurance.Title;
                        string InsuranceComp = SelectedInsurance.InsuranceCompany;
                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 3,
                            AssetId = assetsInsurance.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"{InsuranceTitle}{InsuranceTit} and {InsuranceCompany}{InsuranceComp} ")
                        };
                        _context.AssetLogs.Add(assetLog);
                        _context.SaveChanges();
                        return Ok("Link Asset Insurance Successfully");
                    }
                    catch (Exception e)
                    {

                        return BadRequest(e.Message);
                    }
                }
                return BadRequest("Enter Asset Id..");
            }
            return BadRequest("Enter Insurance Id..");
        }
        [HttpDelete]
        public IActionResult DeleteAssetInsurance(AssetsInsurance assetInsurance)
        {
            if (assetInsurance.InsuranceId != null)
            {
                if (assetInsurance.AssetId != 0)
                {
                    AssetsInsurance _assetInsurance = _context.AssetsInsurances.Where(e => e.InsuranceId == assetInsurance.InsuranceId && e.AssetId == assetInsurance.AssetId).FirstOrDefault();
                    if (_assetInsurance == null)
                    {
                        return BadRequest("Asset Insurance Not Found..");
                    }
                    try
                    {
                        _context.AssetsInsurances.Remove(_assetInsurance);
                        Insurance insurance = _context.Insurances.Find(assetInsurance.InsuranceId);

                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 7,
                            AssetId = assetInsurance.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format($"Dettached Asset Insurance With Insurance Name : {insurance.Title} and Insurance Company : {insurance.InsuranceCompany}")
                        };
                        _context.AssetLogs.Add(assetLog);
                        _context.SaveChanges();
                        return Ok("Successfully Delete Insurance..");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                return BadRequest("Enter Asset Id..");
            }
            return BadRequest("Enter Insurance Id..");
        }
        [HttpGet]
        public IActionResult GetAssetWarrantiesById(int? AssetId, [FromQuery] int TenantId)
        {
            if (AssetId != null && AssetId != 0)
            {
                try
                {
                    var assetwarranties = _context.AssetWarranties.Where(a => a.Asset.TenantId == TenantId && a.AssetId == AssetId).Select(i => new
                    {
                        i.Length,
                        i.ExpirationDate,
                        i.Notes,
                    });
                    return Ok(assetwarranties);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Enter Asset Id..");
        }
        [HttpPost]
        public IActionResult OnPostAddAssetWarranty(AssetWarranty assetWarranty)
        {

            if (assetWarranty.AssetId != 0)
            {
                if (assetWarranty.Length != 0)
                {
                    try
                    {
                        _context.AssetWarranties.Add(assetWarranty);
                        AssetLog assetLog = new AssetLog()
                        {
                            ActionLogId = 20,
                            AssetId = assetWarranty.AssetId,
                            ActionDate = DateTime.Now,
                            Remark = string.Format("Create Warranty")
                        };
                        _context.AssetLogs.Add(assetLog);
                        _context.SaveChanges();
                        return Ok("Asset Warranty Added Successfully..");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                return BadRequest("Enter Warranty Length..");
            }
            return BadRequest("Enter Asset Id..");
        }
        [HttpDelete]
        public IActionResult DeattachAssetWarranty(int? AssetWarrantyId)
        {
            if (AssetWarrantyId != 0 && AssetWarrantyId != null)
            {
                AssetWarranty _assetwarranty = _context.AssetWarranties.FirstOrDefault(a=>a.WarrantyId==AssetWarrantyId);
                if (_assetwarranty == null)
                {
                    return BadRequest("Asset Warranty Not Found..");
                }
                try
                {
                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 21,
                        AssetId = _assetwarranty.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($"Dettached Asset Warranty With Warranty Length: {_assetwarranty.Length} and Expiration Date : {_assetwarranty.ExpirationDate}")
                    };
                    _context.AssetLogs.Add(assetLog);
                    _context.AssetWarranties.Remove(_assetwarranty);
                    _context.SaveChanges();
                    return Ok("Asset Warranty Deattached Successfully..");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Enter Asset Warranty Id..");
        }
        [HttpGet]
        public IActionResult GetAllCategories([FromQuery] int TenantId)
        {
            try
            {
                var Categories = _context.Categories.Where(a => a.TenantId == TenantId).ToList();
                return Ok(Categories);
            }
            catch (Exception)
            {
                return BadRequest("Server Error..");
            }

        }
        [HttpGet]
        public IActionResult GetCategoryById(int? CategoryId, [FromQuery] int TenantId)
        {
            if (CategoryId != null)
            {
                var Category = _context.Categories.Where(a => a.TenantId == TenantId).FirstOrDefault(a=>a.CategoryId==CategoryId);
                if (Category != null)
                {
                    return Ok(Category);
                }
                return BadRequest("Category Not Found..");
            }
            return BadRequest("Enter Category Id..");
        }
        [HttpPost]
        public IActionResult PostAddCategory(Category category)
        {
            if (category.CategoryTIAR != null)
            {
                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return Ok("Created Successfully..");
                }
                catch (Exception)
                {

                    return BadRequest($"Can't Add {category.CategoryTIAR}");
                }
            }
            return BadRequest("");
        }
        [HttpPut]
        public IActionResult PutCategory(int? Categoryid, Category Category, [FromQuery] int TenantId)
        {
            if (Categoryid != null)
            {
                if (Category.CategoryTIAR != null || Category.CategoryTIAR != "")
                {
                    var category = _context.Categories.Where(sup =>sup.TenantId==TenantId && sup.CategoryId == Categoryid).FirstOrDefault();
                    if (category != null)
                    {
                        category.CategoryTIAR = Category.CategoryTIAR;
                        try
                        {
                            _context.SaveChanges();
                            return NoContent();
                        }
                        catch (Exception EX)
                        {
                            return BadRequest(EX.Message);
                        }
                    }
                    return BadRequest("Can't Find Category..");
                }
                return BadRequest("Enter Category Title ..");

            }
            return BadRequest("Enter Category Id ..");

        }
        [HttpDelete]
        public IActionResult DeleteCategory(int Categoryid)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(a=>a.CategoryId==Categoryid);
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Ok("Deleted Successfully..");
            }
            catch (Exception EX)
            {
                return BadRequest(EX.Message);
            }
        }

        [HttpPost]
        public IActionResult PostAddAssetMaintainance(AssetMaintainance assetMaintainance, [FromQuery] int TenantId)
        {
            try
            {
                var assetobj = _context.Assets.Where(e =>e.TenantId == TenantId && e.AssetId == assetMaintainance.AssetId).Include(e => e.AssetStatus).FirstOrDefault();
                if (assetobj == null)
                {
                    return BadRequest("Enter Correct Asset Id..");

                }
                if (assetobj.AssetStatusId != 1 || assetobj.AssetStatusId != 2)
                {
                    return BadRequest($"This Asset noW {assetobj.AssetStatus.AssetStatusTitle}..");
                }
                if (assetMaintainance.AssetId == 0)
                {
                    return BadRequest("Enter Correct Asset Id..");
                }
                if (assetMaintainance.AssetMaintainanceTitle == null)
                {
                    return BadRequest("Asset Maintainance Title Is Required..");
                }
                if (assetMaintainance.AssetMaintainanceRepairesCost <= 0)
                {
                    return BadRequest("Asset Maintainance Repaire Cost Must Be Grater Than 0..");
                }
                if (assetMaintainance.MaintainanceStatusId == null)
                {
                    return BadRequest("Maintainance Status is Required..");

                }
                if (assetMaintainance.MaintainanceStatusId == 5)
                {
                    if (assetMaintainance.AssetMaintainanceDateCompleted != null)
                    {
                        return BadRequest("Asset Maintainance Date Completed Required ..");
                    }
                }
                if (assetMaintainance.AssetMaintainanceDateCompleted < assetMaintainance.ScheduleDate)
                {
                    return BadRequest("Schedule Date Must be less than Completed Date..");
                }
                if (assetMaintainance.TechnicianId == null)
                {
                    return BadRequest("Technican Name Is Required..");
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
                            return BadRequest("Week Frequency Informations Is Required..");
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
                            return BadRequest("Month Frequency Informations Is Required..");
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
                            return BadRequest("Year Frequency Informations Is Required..");
                        }
                    }
                }
                if (ModelState.IsValid)
                {



                    assetMaintainance.AssetMaintainanceDueDate = DateTime.Now;
                    _context.AssetMaintainances.Add(assetMaintainance);
                    assetobj.AssetStatusId = 9;
                    var UpdatedAsset = _context.Assets.Attach(assetobj);
                    UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    string DueDate = assetMaintainance.AssetMaintainanceDueDate?.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    string CompletedDate = assetMaintainance.AssetMaintainanceDateCompleted?.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 18,
                        AssetId = assetMaintainance.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($"Maintainance Asset with Title {assetMaintainance.AssetMaintainanceTitle} and DueDate {DueDate} and Completed Date {CompletedDate}")
                    };
                    _context.AssetLogs.Add(assetLog);
                    _context.SaveChanges();

                    return BadRequest("Asset Maintainance Added successfully");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            return BadRequest("Asset Maintainance Not Added ,Try again");
        }
        [HttpPut]
        public IActionResult OnPostEditAssetMaintainance(AssetMaintainance assetMaintainance)
        {
            try
            {
                if (assetMaintainance.AssetId == 0)
                {
                    return BadRequest("Enter Correct Asset Id..");
                }
                if (assetMaintainance.AssetMaintainanceTitle == null)
                {
                    return BadRequest("Asset Maintainance Title Is Required..");
                }
                if (assetMaintainance.AssetMaintainanceRepairesCost <= 0)
                {
                    return BadRequest("Asset Maintainance Repaire Cost Must Be Grater Than 0..");
                }
                if (assetMaintainance.MaintainanceStatusId == null)
                {
                    return BadRequest("Maintainance Status is Required..");

                }
                if (assetMaintainance.MaintainanceStatusId == 5)
                {
                    if (assetMaintainance.AssetMaintainanceDateCompleted != null)
                    {
                        return BadRequest("Asset Maintainance Date Completed Required ..");
                    }
                }
                if (assetMaintainance.AssetMaintainanceDateCompleted < assetMaintainance.ScheduleDate)
                {
                    return BadRequest("Schedule Date Must be less than Completed Date..");
                }
                if (assetMaintainance.TechnicianId == null)
                {
                    return BadRequest("Technican Name Is Required..");
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
                            return BadRequest("Week Frequency Informations Is Required..");
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
                            return BadRequest("Month Frequency Informations Is Required..");
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
                            return BadRequest("Year Frequency Informations Is Required..");
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    var UpdatedMaintainance = _context.AssetMaintainances.Attach(assetMaintainance);
                    UpdatedMaintainance.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    var Status = _context.MaintainanceStatuses.Find(assetMaintainance.MaintainanceStatusId).MaintainanceStatusTitle;

                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 22,
                        AssetId = assetMaintainance.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format($"Edit Asset Maintainance with Status {Status}")
                    };
                    _context.AssetLogs.Add(assetLog);
                    _context.SaveChanges();
                    return BadRequest("Asset Maintainance Added successfully");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            return BadRequest("Asset Maintainance Not Added ,Try again");
        }

        [HttpGet]
        public IActionResult GetAssetMaintainance(int? AssetId, [FromQuery] int TenantId)
        {
            if (AssetId != null)
            {
                try
                {
                    var assetmaintainances = _context.AssetMaintainances.Where(a =>a.Asset.TenantId==TenantId && a.AssetId == AssetId).Select(i => new
                    {
                        i.AssetMaintainanceId,
                        i.AssetMaintainanceTitle,
                        i.AssetMaintainanceDetails,
                        i.AssetMaintainanceDueDate,
                        i.MaintainanceStatusId,
                        i.AssetMaintainanceDateCompleted,
                        i.AssetMaintainanceRepairesCost,
                        i.AssetMaintainanceRepeating,
                        i.AssetMaintainanceFrequencyId,
                        i.TechnicianId,
                        i.AssetId,
                        i.WeeklyPeriod,
                        i.WeekDayId,
                        i.MonthlyPeriod,
                        i.MonthlyDay,
                        i.MonthId,
                        i.YearlyDay,
                        i.ScheduleDate
                    });
                    return Ok(assetmaintainances);

                }
                catch (Exception e)
                {

                    return BadRequest(e.Message);

                }

            }
            return BadRequest("Enter Asset Id..");
        }
        [HttpGet]
        public IActionResult GetDocumentById(int? DocumentId, [FromQuery] int TenantId)
        {
            if (DocumentId != null)
            {
                try
                {
                    var Document = _context.assetDocuments.Where(a =>a.Asset.TenantId==TenantId && a.AssetDocumentId == DocumentId).Select(i => new
                    {
                        i.DocumentName,
                        i.DocumentType,
                        i.Description,
                        i.Asset.AssetCost,
                        i.Asset.AssetTagId,
                        i.Asset.AssetSerialNo,
                        i.Asset.Photo,
                    });
                    return Ok(Document);
                }
                catch (Exception e)
                {

                    return BadRequest(e.Message);

                }

            }
            return BadRequest("Enter Document Id..");
        }
        [HttpGet]
        public IActionResult GetContractById(int? ContractId,[FromQuery] int TenantId)
        {

            if (ContractId != null)
            {
                try
                {
                    var contract = _context.AssetContracts.Where(e =>e.Asset.TenantId==TenantId && e.AssetContractID == ContractId).Select(i => new
                    {
                        i.Contract.Description,
                        i.Contract.ContractNo,
                        i.Contract.Cost,
                        i.Contract.StartDate,
                        i.Contract.EndDate,
                        i.Contract.Title,
                        i.Contract.Vendor.VendorTitle,
                        i.Asset.AssetCost,
                        i.Asset.AssetTagId,
                        i.Asset.AssetSerialNo,
                        i.Asset.Photo
                    });
                    return Ok(contract);

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Enter Contract Id.. ");
        }
        [HttpGet]
        public IActionResult GetWarrantyById(int? WarrantyId, [FromQuery] int TenantId)
        {
            if (WarrantyId != null)
            {
                try
                {
                    var Warranty = _context.AssetWarranties.Where(a =>a.Asset.TenantId==TenantId && a.WarrantyId == WarrantyId).Select(i => new
                    {
                        i.Length,
                        i.ExpirationDate,
                        i.Notes,
                        i.Asset.AssetCost,
                        i.Asset.AssetTagId,
                        i.Asset.AssetSerialNo,
                        i.Asset.Photo,
                    });
                    return Ok(Warranty);
                }
                catch (Exception e)
                {

                    return BadRequest(e.Message);

                }

            }
            return BadRequest("Enter Warranty Id..");
        }
        [HttpPost]
        public IActionResult PostLogOut()
        {
            _signInManager.SignOutAsync();
            return Ok();
        }

        
        [HttpPost]
        public IActionResult PostcheckedoutassetsFromDepartment([FromBody] AssetMovement assetmovement, [FromBody] List<Asset> SelectedAssets)
        {
           
            if (assetmovement.LocationId == null)
            {
                return BadRequest("Please Select Location..");
            }
            if (assetmovement.DepartmentId == null)
            {
                return BadRequest("Please Select Department..");
            }
            if (assetmovement.EmpolyeeID == null)
            {
                return BadRequest("Please Select Employee..");
            }
            if (assetmovement.StoreId == null)
            {
                return BadRequest("Please Select Store..");
            }
             if (assetmovement.TransactionDate == null)
            {
                return BadRequest("Please Enter TransActionDate..");
            }

            if (SelectedAssets != null)
            {
                int CheckInID = checkinAssetsfromDepartmentTostore(assetmovement, SelectedAssets);
                if (CheckInID == 0)
                {
                    return BadRequest("Something went Error,Try again");
                }
                //Second move asset from store to department
                int CheckoutID = checkoutAssetsToEmpolyee(assetmovement, SelectedAssets);
                if (CheckoutID == 0)
                {
                    return BadRequest("Something went Error,Try again");
                }

                //Print check in form
                return Ok("Asset Movements Added successfully");
                //Print check out form

            }
            return BadRequest("Please Select at Least one Asset");

        }
       
        [ApiExplorerSettings(IgnoreApi = true)]
        public int checkinAssetsfromDepartmentTostore(AssetMovement assetMovementObj, List<Asset> selectedAssetsList)
        {
            AssetMovement newAssetMovement = null;
            if (selectedAssetsList.Count != 0)
            {
                newAssetMovement = new AssetMovement()
                {
                    AssetMovementDirectionId = 2,
                    ActionTypeId = 2,
                    DepartmentId = assetMovementObj.DepartmentId,
                    LocationId = assetMovementObj.LocationId,
                    TransactionDate = assetMovementObj.TransactionDate,
                    //DueDate=assetMovementObj.DueDate,
                    Remarks = assetMovementObj.Remarks,
                    StoreId = assetMovementObj.StoreId,
                };
                newAssetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                string DirectionTitle = "Direction Title : ";
                string TransDate = "Transaction Date : ";
                AssetMovementDirection Direction = _context.AssetMovementDirections.Find(newAssetMovement.AssetMovementDirectionId);
                string TransactionDate = assetMovementObj.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
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
                catch (Exception)
                {
                    return 0;
                }
            }
            return newAssetMovement.AssetMovementId;
        }
       
        [ApiExplorerSettings(IgnoreApi = true)]
        public int checkoutAssetsToEmpolyee(AssetMovement assetMovementObj, List<Asset> selectedAssetsList)
        {
            AssetMovement newAssetMovement = null;
            if (selectedAssetsList.Count != 0)
            {
                newAssetMovement = new AssetMovement()
                {
                    AssetMovementDirectionId = 1,
                    ActionTypeId = 1,
                    DepartmentId = assetMovementObj.DepartmentId,
                    LocationId = assetMovementObj.LocationId,
                    StoreId = assetMovementObj.StoreId,
                    Remarks = assetMovementObj.Remarks,
                    TransactionDate = assetMovementObj.TransactionDate,
                    EmpolyeeID = assetMovementObj.EmpolyeeID

                    //DueDate=assetMovementObj.DueDate
                };

                newAssetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                string ActionTitle = "Action Title : ";
                string TransDate = "Transaction Date : ";
                string DirectionTitle = "Direction Title : ";
                ActionType SelectedActionType = _context.ActionTypes.Find(newAssetMovement.ActionTypeId);
                AssetMovementDirection Direction = _context.AssetMovementDirections.Find(newAssetMovement.AssetMovementDirectionId);
                string TransactionDate = assetMovementObj.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
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
                catch (Exception)
                {
                    return 0;
                }
            }
            return newAssetMovement.AssetMovementId;
        }
       
        
        [HttpPost]
        public IActionResult PostcheckedoutassetsFromEmployee([FromBody] AssetMovement assetmovement, [FromBody] List<Asset> SelectedAssets)
        {
            if (assetmovement.LocationId == null)
            {
                return BadRequest("Please Select Location..");
            }
            if (assetmovement.DepartmentId == null)
            {
                return BadRequest("Please Select Department..");
            }
            if (assetmovement.EmpolyeeID == null)
            {
                return BadRequest("Please Select Employee..");
            }

            if (assetmovement.StoreId == null)
            {
                return BadRequest("Please Select Store..");
            }
            if (assetmovement.TransactionDate == null)
            {
                return BadRequest("Please Enter TransActionDate..");
            }

            //Inert two movement

            //First move assets to store --> Check in
            if (SelectedAssets != null)
            {
                int CheckInID = checkinAssetsfromEmpolyeeTostore(assetmovement, SelectedAssets);
                if (CheckInID == 0)
                {
                    return BadRequest("Something went Error,Try again");
                }

                //Second move asset from store to department
                int CheckoutID = checkoutAssetsToDepartment(assetmovement, SelectedAssets);
                if (CheckoutID == 0)
                {
                    return BadRequest("Something went Error,Try again");
                }

                //Print check in form
                return Ok("Asset Movements Added successfully");
                //Print check out form

            }
            return BadRequest("Please Select at Least one Asset");
        }
       
        [ApiExplorerSettings(IgnoreApi = true)]
        public int checkinAssetsfromEmpolyeeTostore(AssetMovement assetMovementObj, List<Asset> selectedAssetsList)
        {
            AssetMovement newAssetMovement = null;
            if (selectedAssetsList.Count != 0)
            {
                var LastassetmovementForEmpolyee = _context.AssetMovements.Where(a => a.EmpolyeeID == assetMovementObj.EmpolyeeID && a.AssetMovementDirectionId == 1).OrderByDescending(a => a.AssetMovementId).FirstOrDefault();
                newAssetMovement = new AssetMovement()
                {
                    AssetMovementDirectionId = 2,
                    ActionTypeId = 1,
                    DepartmentId = LastassetmovementForEmpolyee.DepartmentId,
                    LocationId = LastassetmovementForEmpolyee.LocationId,
                    TransactionDate = assetMovementObj.TransactionDate,
                    //DueDate=assetMovementObj.DueDate,
                    Remarks = assetMovementObj.Remarks,
                    StoreId = assetMovementObj.StoreId,
                    EmpolyeeID = assetMovementObj.EmpolyeeID
                };


                newAssetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                string DirectionTitle = "Direction Title : ";
                string TransDate = "Transaction Date : ";
                AssetMovementDirection Direction = _context.AssetMovementDirections.Find(newAssetMovement.AssetMovementDirectionId);
                string TransactionDate = assetMovementObj.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
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
       
        [ApiExplorerSettings(IgnoreApi = true)]
        public int checkoutAssetsToDepartment(AssetMovement assetMovementObj, List<Asset> selectedAssetsList)
        {
            AssetMovement newAssetMovement = null;
            if (selectedAssetsList.Count != 0)
            {
                newAssetMovement = new AssetMovement()
                {
                    AssetMovementDirectionId = 1,
                    ActionTypeId = 2,
                    DepartmentId = assetMovementObj.DepartmentId,
                    LocationId = assetMovementObj.LocationId,
                    StoreId = assetMovementObj.StoreId,
                    Remarks = assetMovementObj.Remarks,
                    TransactionDate = assetMovementObj.TransactionDate
                    //DueDate=assetMovementObj.DueDate
                };

                newAssetMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                string ActionTitle = "Action Title : ";
                string TransDate = "Transaction Date : ";
                string DirectionTitle = "Direction Title : ";
                ActionType SelectedActionType = _context.ActionTypes.Find(newAssetMovement.ActionTypeId);
                AssetMovementDirection Direction = _context.AssetMovementDirections.Find(newAssetMovement.AssetMovementDirectionId);
                string TransactionDate = assetMovementObj.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
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

        //[HttpPost]
        //public IActionResult TransferFrom([FromBody] TwowaysTansfer twoways )
        //{

        //    if (twoways.LeftActionTypeId == null)
        //    {
        //        return Ok(new { Message = "Please Select Action Type!", status = false });
        //    }
        //    else if (twoways.LeftActionTypeId == 1)
        //    {
        //        if (twoways.LeftDepartmentId == null)
        //        {
        //            return Ok(new { Message = "Please Select Department!", status = false });
        //        }
        //        if (twoways.LeftEmployeeId == null)
        //        {
        //            return Ok(new { Message = "Please Select Empolyee!", status = false });
        //        }

        //    }
        //    else if (twoways.LeftActionTypeId == 2)
        //    {
        //        if (twoways.LeftDepartmentId == null)
        //        {
        //            return Ok(new { Message = "Please Select Department!", status = false });
        //        }

        //    }
        //    if (twoways.LeftLocationId == null)
        //    {
        //        return Ok(new { Message = "Please Select Location!", status = false });
        //    }

        //    if (twoways.LeftStoreId == null)
        //    {
        //        return Ok(new { Message = "Please Select Store!", status = false });
        //    }

        //    if (twoways.RightActionTypeId == null)
        //    {
        //        return Ok(new { Message = "Please Select Action!", status = false });
        //    }
        //    else if (twoways.RightActionTypeId == 1)
        //    {
        //        if (twoways.RightDepartmentId == null)
        //        {
        //            return Ok(new { Message = "Please Select Department!", status = false });
        //        }
        //        if (twoways.RightEmployeeId == null)
        //        {
        //            return Ok(new { Message = "Please Select Empolyee!", status = false });
        //        }

        //    }
        //    else if (twoways.RightActionTypeId == 2)
        //    {
        //        if (twoways.RightDepartmentId == null)
        //        {
        //            return Ok(new { Message = "Please Select Department!", status = false });
        //        }


        //    }

        //    if (twoways.RightLocationId == null)
        //    {
        //        return Ok(new { Message = "Please Select Location!", status = false });
        //    }

        //    if (twoways.RightStoreId == null)
        //    {
        //        return Ok(new { Message = "Please Select Store!", status = false });
        //    }

        //    //Insert two movement
        //    if (twoways.SelectedLeftAssets == null && twoways.SelectedRightAssets == null && twoways.SelectedLeftAssets.Count == 0 && twoways.SelectedRightAssets.Count == 0)
        //    {

        //        return Ok(new { Message = "Please!Select at least one Asset from anySide to transfer!", status = false });
        //    }
        //    if (twoways.SelectedLeftAssets.Count != 0)
        //    {
        //        bool exist = false;
        //        foreach (var item in twoways.SelectedLeftAssets)
        //        {
        //            exist = twoways.RightDataSource.Any(e => e.AssetId == item.AssetId);
        //            if (!exist)
        //            {
        //                break;
        //            }
        //        }
        //        if (exist)
        //        {
        //            if (twoways.RightActionTypeId == 1)
        //            {

        //                foreach (var item in twoways.RightEmployeeDataSource.Distinct())
        //                {
        //                    if (twoways.SelectedLeftAssets.Any(e => e.AssetId == item.AssetId))
        //                    {
        //                        var removeitem = twoways.SelectedLeftAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
        //                        twoways.SelectedLeftAssets.Remove(removeitem);
        //                    }
        //                }
        //            }
        //            if (twoways.RightActionTypeId == 2)
        //            {
        //                foreach (var item in twoways.RightDepartmentDataSource.Distinct())
        //                {
        //                    if (twoways.SelectedLeftAssets.Any(e => e.AssetId == item.AssetId))
        //                    {
        //                        var removeitem = twoways.SelectedLeftAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
        //                        twoways.SelectedLeftAssets.Remove(removeitem);
        //                    }
        //                }
        //            }
        //            //checkIn Left Assets To Store
        //            AssetMovement LeftCheckInMovement = null;

        //            if (twoways.LeftActionTypeId == 1)
        //            {
        //                LeftCheckInMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 2,
        //                    EmpolyeeID = twoways.LeftEmployeeId,
        //                    ActionTypeId = 1,
        //                    DepartmentId = twoways.LeftDepartmentId,
        //                    LocationId = twoways.LeftLocationId,
        //                    TransactionDate = DateTime.Now,
        //                    StoreId = twoways.LeftStoreId,
        //                };
        //            }
        //            else if (twoways.LeftActionTypeId == 2)
        //            {
        //                LeftCheckInMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 2,
        //                    DepartmentId = twoways.LeftDepartmentId,
        //                    ActionTypeId = 2,
        //                    LocationId = twoways.LeftLocationId,
        //                    TransactionDate = DateTime.Now,
        //                    StoreId = twoways.LeftStoreId,
        //                };
        //            }

        //            LeftCheckInMovement.AssetMovementDetails = new List<AssetMovementDetails>();
        //            string LeftCheckInDirectionTitle = "Direction Title : ";
        //            string LeftCheckInTransDate = "Transaction Date : ";
        //            AssetMovementDirection LeftCheckInDirection = _context.AssetMovementDirections.Find(LeftCheckInMovement.AssetMovementDirectionId);
        //            string TransactionDate = LeftCheckInMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        //            foreach (var asset in twoways.SelectedLeftAssets)
        //            {
        //                asset.AssetStatusId = 1;
        //                var UpdatedAsset = _context.Assets.Attach(asset);
        //                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //                LeftCheckInMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });
        //                AssetLog assetLog = new AssetLog()
        //                {
        //                    ActionLogId = 16,
        //                    AssetId = asset.AssetId,
        //                    ActionDate = DateTime.Now,
        //                    Remark = string.Format($"{LeftCheckInTransDate}{TransactionDate} and {LeftCheckInDirectionTitle}{LeftCheckInDirection.AssetMovementDirectionTitle} Transfered")
        //                };
        //                _context.AssetLogs.Add(assetLog);
        //            }

        //            _context.AssetMovements.Add(LeftCheckInMovement);
        //            try
        //            {
        //                _context.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                return Ok(new { Message = "Something went Error,Try again", status = false, error=e.Message });
        //            }
        //            //checkOut Left Assets To Store

        //            AssetMovement LeftCheckOutMovement = null;

        //            if (twoways.RightActionTypeId == 1)
        //            {
        //                LeftCheckOutMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 1,
        //                    ActionTypeId = 1,
        //                    DepartmentId = twoways.RightDepartmentId,
        //                    LocationId = twoways.RightLocationId,
        //                    StoreId = twoways.LeftStoreId,
        //                    TransactionDate = DateTime.Now,
        //                    EmpolyeeID = twoways.RightEmployeeId,
        //                };

        //            }
        //            else if (twoways.RightActionTypeId == 2)
        //            {
        //                LeftCheckOutMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 1,
        //                    ActionTypeId = 2,
        //                    DepartmentId = twoways.RightDepartmentId,
        //                    LocationId = twoways.RightLocationId,
        //                    StoreId = twoways.LeftStoreId,
        //                    TransactionDate = DateTime.Now,
        //                };
        //            }

        //            LeftCheckOutMovement.AssetMovementDetails = new List<AssetMovementDetails>();
        //            string ActionTitle = "Action Title : ";
        //            string LeftCheckoutTransDate = "Transaction Date : ";
        //            string LeftCheckOutDirectionTitle = "Direction Title : ";
        //            ActionType SelectedActionType = _context.ActionTypes.Find(LeftCheckOutMovement.ActionTypeId);
        //            AssetMovementDirection LeftCheckOutDirection = _context.AssetMovementDirections.Find(LeftCheckOutMovement.AssetMovementDirectionId);
        //            string CheckoutTransactionDate = LeftCheckOutMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        //            foreach (var asset in twoways.SelectedLeftAssets)
        //            {
        //                asset.AssetStatusId = 2;
        //                var UpdatedAsset = _context.Assets.Attach(asset);
        //                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //                LeftCheckOutMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });

        //                AssetLog assetLog = new AssetLog()
        //                {
        //                    ActionLogId = 17,
        //                    AssetId = asset.AssetId,
        //                    ActionDate = DateTime.Now,
        //                    Remark = string.Format($"{LeftCheckoutTransDate}{TransactionDate} and {ActionTitle}{SelectedActionType.ActionTypeTitle} and {LeftCheckOutDirectionTitle}{LeftCheckOutDirection.AssetMovementDirectionTitle} Transfered")
        //                };
        //                _context.AssetLogs.Add(assetLog);
        //            }
        //            _context.AssetMovements.Add(LeftCheckOutMovement);
        //            try
        //            {
        //                _context.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
        //            }
        //            twoways.SelectedLeftAssets = new List<Asset>();
        //        }

        //    }

        //    if (twoways.SelectedRightAssets.Count != 0)
        //    {
        //        bool exist = false;
        //        foreach (var item in twoways.SelectedRightAssets)
        //        {
        //            exist = twoways.LeftDataSource.Any(e => e.AssetId == item.AssetId);
        //            if (!exist)
        //            {
        //                break;
        //            }
        //        }
        //        if (exist)
        //        {
        //            //checkIn Right Assets To Store
        //            AssetMovement RightCheckInMovement = null;
        //            if (twoways.LeftActionTypeId == 1)
        //            {

        //                foreach (var item in twoways.LeftEmployeeDataSource.Distinct())
        //                {
        //                    if (twoways.SelectedRightAssets.Any(e => e.AssetId == item.AssetId))
        //                    {
        //                        var removeitem = twoways.SelectedRightAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
        //                        twoways.SelectedRightAssets.Remove(removeitem);
        //                    }
        //                }
        //            }
        //            if (twoways.LeftActionTypeId == 2)
        //            {
        //                foreach (var item in twoways.LeftDepartmentDataSource.Distinct())
        //                {
        //                    if (twoways.SelectedRightAssets.Any(e => e.AssetId == item.AssetId))
        //                    {
        //                        var removeitem = twoways.SelectedRightAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
        //                        twoways.SelectedRightAssets.Remove(removeitem);
        //                    }
        //                }
        //            }
        //            if (twoways.RightActionTypeId == 1)
        //            {
        //                RightCheckInMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 2,
        //                    EmpolyeeID = twoways.RightEmployeeId,
        //                    ActionTypeId = 1,
        //                    DepartmentId = twoways.RightDepartmentId,
        //                    LocationId = twoways.RightLocationId,
        //                    TransactionDate = DateTime.Now,
        //                    StoreId = twoways.RightStoreId,
        //                };
        //            }
        //            else if (twoways.RightActionTypeId == 2)
        //            {
        //                RightCheckInMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 2,
        //                    DepartmentId = twoways.RightDepartmentId,
        //                    ActionTypeId = 2,
        //                    LocationId = twoways.RightLocationId,
        //                    TransactionDate = DateTime.Now,
        //                    StoreId = twoways.RightStoreId,
        //                };
        //            }

        //            RightCheckInMovement.AssetMovementDetails = new List<AssetMovementDetails>();
        //            string RightCheckInDirectionTitle = "Direction Title : ";
        //            string RightCheckInTransDate = "Transaction Date : ";
        //            AssetMovementDirection RightCheckInDirection = _context.AssetMovementDirections.Find(RightCheckInMovement.AssetMovementDirectionId);
        //            string TransactionDate = RightCheckInMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        //            foreach (var asset in twoways.SelectedRightAssets)
        //            {
        //                asset.AssetStatusId = 1;
        //                var UpdatedAsset = _context.Assets.Attach(asset);
        //                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //                RightCheckInMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });
        //                AssetLog assetLog = new AssetLog()
        //                {
        //                    ActionLogId = 16,
        //                    AssetId = asset.AssetId,
        //                    ActionDate = DateTime.Now,
        //                    Remark = string.Format($"{RightCheckInTransDate}{TransactionDate} and {RightCheckInDirectionTitle}{RightCheckInDirection.AssetMovementDirectionTitle} Transfered")
        //                };
        //                _context.AssetLogs.Add(assetLog);
        //            }

        //            _context.AssetMovements.Add(RightCheckInMovement);
        //            try
        //            {
        //                _context.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
        //            }

        //            //checkOut Right Assets To Store

        //            AssetMovement RightCheckOutMovement = null;

        //            if (twoways.LeftActionTypeId == 1)
        //            {
        //                RightCheckOutMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 1,
        //                    ActionTypeId = 1,
        //                    DepartmentId = twoways.LeftDepartmentId,
        //                    LocationId = twoways.LeftLocationId,
        //                    StoreId = twoways.RightStoreId,
        //                    TransactionDate = DateTime.Now,
        //                    EmpolyeeID = twoways.LeftEmployeeId,
        //                };

        //            }
        //            else if (twoways.LeftActionTypeId == 2)
        //            {
        //                RightCheckOutMovement = new AssetMovement()
        //                {
        //                    AssetMovementDirectionId = 1,
        //                    ActionTypeId = 2,
        //                    DepartmentId = twoways.LeftDepartmentId,
        //                    LocationId = twoways.LeftLocationId,
        //                    StoreId = twoways.RightStoreId,
        //                    TransactionDate = DateTime.Now,
        //                };
        //            }
        //             RightCheckOutMovement.AssetMovementDetails = new List<AssetMovementDetails>();
        //            string ActionTitle = "Action Title : ";
        //            string RightCheckoutTransDate = "Transaction Date : ";
        //            string RightCheckOutDirectionTitle = "Direction Title : ";
        //            ActionType SelectedActionType = _context.ActionTypes.Find(RightCheckOutMovement.ActionTypeId);
        //            AssetMovementDirection RightCheckOutDirection = _context.AssetMovementDirections.Find(RightCheckOutMovement.AssetMovementDirectionId);
        //            string CheckoutTransactionDate = RightCheckOutMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        //            foreach (var asset in twoways.SelectedRightAssets)
        //            {
        //                asset.AssetStatusId = 2;
        //                var UpdatedAsset = _context.Assets.Attach(asset);
        //                UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //                RightCheckOutMovement.AssetMovementDetails.Add(new AssetMovementDetails() { AssetId = asset.AssetId, Remarks = "" });

        //                AssetLog assetLog = new AssetLog()
        //                {
        //                    ActionLogId = 17,
        //                    AssetId = asset.AssetId,
        //                    ActionDate = DateTime.Now,
        //                    Remark = string.Format($"{RightCheckoutTransDate}{TransactionDate} and {ActionTitle}{SelectedActionType.ActionTypeTitle} and {RightCheckOutDirectionTitle}{RightCheckOutDirection.AssetMovementDirectionTitle} Transfered")
        //                };
        //                _context.AssetLogs.Add(assetLog);
        //            }
        //            _context.AssetMovements.Add(RightCheckOutMovement);
        //            try
        //            {
        //                _context.SaveChanges();

        //            }
        //            catch (Exception e)
        //            {
        //                return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
        //            }

        //        }
        //        twoways.SelectedRightAssets = new List<Asset>();

        //    }
        //    return Ok(new { Message = "Transaction Added Successfully", status = true });
        //}





        [HttpPost]
        public IActionResult TransferFrom([FromBody] TwowaysTansferTo twoways)
        {

            if (twoways.LeftActionTypeId == null)
            {
                return Ok(new { Message = "Please Select Action Type!", status = false });
            }
            else if (twoways.LeftActionTypeId == 1)
            {
                if (twoways.LeftDepartmentId == null)
                {
                    return Ok(new { Message = "Please Select Department!", status = false });
                }
                if (twoways.LeftEmployeeId == null)
                {
                    return Ok(new { Message = "Please Select Empolyee!", status = false });
                }

            }
            else if (twoways.LeftActionTypeId == 2)
            {
                if (twoways.LeftDepartmentId == null)
                {
                    return Ok(new { Message = "Please Select Department!", status = false });
                }

            }
            if (twoways.LeftLocationId == null)
            {
                return Ok(new { Message = "Please Select Location!", status = false });
            }

            if (twoways.LeftStoreId == null)
            {
                return Ok(new { Message = "Please Select Store!", status = false });
            }

            if (twoways.RightActionTypeId == null)
            {
                return Ok(new { Message = "Please Select Action!", status = false });
            }
            else if (twoways.RightActionTypeId == 1)
            {
                if (twoways.RightDepartmentId == null)
                {
                    return Ok(new { Message = "Please Select Department!", status = false });
                }
                if (twoways.RightEmployeeId == null)
                {
                    return Ok(new { Message = "Please Select Empolyee!", status = false });
                }

            }
            else if (twoways.RightActionTypeId == 2)
            {
                if (twoways.RightDepartmentId == null)
                {
                    return Ok(new { Message = "Please Select Department!", status = false });
                }


            }

            if (twoways.RightLocationId == null)
            {
                return Ok(new { Message = "Please Select Location!", status = false });
            }

            if (twoways.RightStoreId == null)
            {
                return Ok(new { Message = "Please Select Store!", status = false });
            }

            //Insert two movement
            if (twoways.SelectedLeftAssets == null && twoways.SelectedRightAssets == null && twoways.SelectedLeftAssets.Count == 0 && twoways.SelectedRightAssets.Count == 0)
            {

                return Ok(new { Message = "Please!Select at least one Asset from anySide to transfer!", status = false });
            }
            if (twoways.SelectedLeftAssets.Count != 0)
            {
                bool exist = false;
                foreach (var item in twoways.SelectedLeftAssets)
                {
                    exist = twoways.RightDataSource.Any(e => e.AssetId == item.AssetId);
                    if (!exist)
                    {
                        break;
                    }
                }
                if (exist)
                {
                    if (twoways.RightActionTypeId == 1)
                    {

                        foreach (var item in twoways.RightEmployeeDataSource.Distinct())
                        {
                            if (twoways.SelectedLeftAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = twoways.SelectedLeftAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                twoways.SelectedLeftAssets.Remove(removeitem);
                            }
                        }
                    }
                    if (twoways.RightActionTypeId == 2)
                    {
                        foreach (var item in twoways.RightDepartmentDataSource.Distinct())
                        {
                            if (twoways.SelectedLeftAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = twoways.SelectedLeftAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                twoways.SelectedLeftAssets.Remove(removeitem);
                            }
                        }
                    }
                    List<Asset> assetList = new List<Asset>();
                    //checkIn Left Assets To Store
                    AssetMovement LeftCheckInMovement = null;

                    if (twoways.LeftActionTypeId == 1)
                    {
                        LeftCheckInMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 2,
                            EmpolyeeID = twoways.LeftEmployeeId,
                            ActionTypeId = 1,
                            DepartmentId = twoways.LeftDepartmentId,
                            LocationId = twoways.LeftLocationId,
                            TransactionDate = DateTime.Now,
                            StoreId = twoways.LeftStoreId,
                        };
                    }
                    else if (twoways.LeftActionTypeId == 2)
                    {
                        LeftCheckInMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 2,
                            DepartmentId = twoways.LeftDepartmentId,
                            ActionTypeId = 2,
                            LocationId = twoways.LeftLocationId,
                            TransactionDate = DateTime.Now,
                            StoreId = twoways.LeftStoreId,
                        };
                    }

                    LeftCheckInMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                    string LeftCheckInDirectionTitle = "Direction Title : ";
                    string LeftCheckInTransDate = "Transaction Date : ";
                    AssetMovementDirection LeftCheckInDirection = _context.AssetMovementDirections.Find(LeftCheckInMovement.AssetMovementDirectionId);
                    string TransactionDate = LeftCheckInMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    foreach (var item in twoways.SelectedLeftAssets)
                    {
                        var model = _mapper.Map<Asset>(item);
                        assetList.Add(model);
                    }
                    foreach (var asset in assetList)
                    {
                        asset.AssetStatusId = 1;
                        //asset.DepreciationMethodId = null;
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
                        return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
                    }
                    //checkOut Left Assets To Store

                    AssetMovement LeftCheckOutMovement = null;

                    if (twoways.RightActionTypeId == 1)
                    {
                        LeftCheckOutMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 1,
                            ActionTypeId = 1,
                            DepartmentId = twoways.RightDepartmentId,
                            LocationId = twoways.RightLocationId,
                            StoreId = twoways.LeftStoreId,
                            TransactionDate = DateTime.Now,
                            EmpolyeeID = twoways.RightEmployeeId,
                        };

                    }
                    else if (twoways.RightActionTypeId == 2)
                    {
                        LeftCheckOutMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 1,
                            ActionTypeId = 2,
                            DepartmentId = twoways.RightDepartmentId,
                            LocationId = twoways.RightLocationId,
                            StoreId = twoways.LeftStoreId,
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
                    foreach (var asset in assetList)
                    {
                        asset.AssetStatusId = 2;
                        //asset.DepreciationMethodId = null;
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
                        return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
                    }
                    twoways.SelectedLeftAssets = new List<AssetVm2>();
                }

            }

            if (twoways.SelectedRightAssets.Count != 0)
            {
                bool exist = false;
                foreach (var item in twoways.SelectedRightAssets)
                {
                    exist = twoways.LeftDataSource.Any(e => e.AssetId == item.AssetId);
                    if (!exist)
                    {
                        break;
                    }
                }
                if (exist)
                {
                    //checkIn Right Assets To Store
                    AssetMovement RightCheckInMovement = null;
                    if (twoways.LeftActionTypeId == 1)
                    {

                        foreach (var item in twoways.LeftEmployeeDataSource.Distinct())
                        {
                            if (twoways.SelectedRightAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = twoways.SelectedRightAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                twoways.SelectedRightAssets.Remove(removeitem);
                            }
                        }
                    }
                    if (twoways.LeftActionTypeId == 2)
                    {
                        foreach (var item in twoways.LeftDepartmentDataSource.Distinct())
                        {
                            if (twoways.SelectedRightAssets.Any(e => e.AssetId == item.AssetId))
                            {
                                var removeitem = twoways.SelectedRightAssets.FirstOrDefault(e => e.AssetId == item.AssetId);
                                twoways.SelectedRightAssets.Remove(removeitem);
                            }
                        }
                    }
                    if (twoways.RightActionTypeId == 1)
                    {
                        RightCheckInMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 2,
                            EmpolyeeID = twoways.RightEmployeeId,
                            ActionTypeId = 1,
                            DepartmentId = twoways.RightDepartmentId,
                            LocationId = twoways.RightLocationId,
                            TransactionDate = DateTime.Now,
                            StoreId = twoways.RightStoreId,
                        };
                    }
                    else if (twoways.RightActionTypeId == 2)
                    {
                        RightCheckInMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 2,
                            DepartmentId = twoways.RightDepartmentId,
                            ActionTypeId = 2,
                            LocationId = twoways.RightLocationId,
                            TransactionDate = DateTime.Now,
                            StoreId = twoways.RightStoreId,
                        };
                    }
                    List<Asset> assetList2 = new List<Asset>();


                    RightCheckInMovement.AssetMovementDetails = new List<AssetMovementDetails>();
                    string RightCheckInDirectionTitle = "Direction Title : ";
                    string RightCheckInTransDate = "Transaction Date : ";
                    AssetMovementDirection RightCheckInDirection = _context.AssetMovementDirections.Find(RightCheckInMovement.AssetMovementDirectionId);
                    string TransactionDate = RightCheckInMovement.TransactionDate.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                    foreach (var item in twoways.SelectedRightAssets)
                    {
                        var model = _mapper.Map<Asset>(item);
                        assetList2.Add(model);
                    }

                    foreach (var asset in assetList2)
                    {
                        asset.AssetStatusId = 1;
                        asset.DepreciationMethodId = null;
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
                        return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
                    }

                    //checkOut Right Assets To Store

                    AssetMovement RightCheckOutMovement = null;

                    if (twoways.LeftActionTypeId == 1)
                    {
                        RightCheckOutMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 1,
                            ActionTypeId = 1,
                            DepartmentId = twoways.LeftDepartmentId,
                            LocationId = twoways.LeftLocationId,
                            StoreId = twoways.RightStoreId,
                            TransactionDate = DateTime.Now,
                            EmpolyeeID = twoways.LeftEmployeeId,
                        };

                    }
                    else if (twoways.LeftActionTypeId == 2)
                    {
                        RightCheckOutMovement = new AssetMovement()
                        {
                            AssetMovementDirectionId = 1,
                            ActionTypeId = 2,
                            DepartmentId = twoways.LeftDepartmentId,
                            LocationId = twoways.LeftLocationId,
                            StoreId = twoways.RightStoreId,
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
                    foreach (var item in twoways.SelectedRightAssets)
                    {
                        var model = _mapper.Map<Asset>(item);
                        assetList2.Add(model);
                    }

                    foreach (var asset in assetList2)
                    {
                        asset.AssetStatusId = 2;
                        asset.DepreciationMethodId = null;
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
                        return Ok(new { Message = "Something went Error,Try again", status = false, error = e.Message });
                    }

                }
                twoways.SelectedRightAssets = new List<AssetVm2>();

            }
            return Ok(new { Message = "Transaction Added Successfully", status = true });
        }

        [HttpPost]
        public async Task<IActionResult> PatchAddAsset(Asset Asset)
        {
             List<Asset> Assets = new List<Asset>();
        

            if (Asset.ItemId == 0)
            {
                return Ok(new {Message= "Please Select Item",status=false });
            }
            if (Asset.StoreId == null)
            {
                return Ok(new { Message = "Please Select Store", status = false });
            }
            if (Asset.VendorId == null)
            {
                return Ok(new { Message = "Please Select Vendor", status = false });
            }
            if (Asset.DepreciableAsset)
            {
                if (Asset.DepreciationMethodId == null)
                {
                    return Ok(new { Message = "Please Select Depreciation Method", status = false });
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
                    if (Asset.Photo != string.Empty)
                    {
                        var bytes = Convert.FromBase64String(Asset.Photo);
                        string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/AssetPhotos");
                        string uniqePictureName = Guid.NewGuid().ToString("N") + ".jpeg";
                        string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                        using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        Asset.Photo = uniqePictureName;
                    }

                    asset.AssetStatusId = 1;
                    asset.TenantId = Asset.TenantId;
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
                    return Ok(new { Message = "Something went Error", status = false });
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
                    return Ok(new { Message = "Asset Added successfully", status = true, Assets });
                }
                catch (Exception e)
                {
                    return Ok(new { Message = "Something went Error", status = false });
                }

            }
            return Ok(new { Message = "Data Not Valid", status = false });
        }

        [HttpPost]
        public async Task<IActionResult> AddAsset(Asset Asset)
        {
            if (Asset.ItemId == 0)
            {
                return Ok(new { Message = "Please Select Item", status = false });
            }
            if (Asset.StoreId == null)
            {
                return Ok(new { Message = "Please Select Store", status = false });
            }
            if (Asset.VendorId == null)
            {
                return Ok(new { Message = "Please Select Vendor", status = false });
            }
            if (Asset.DepreciableAsset)
            {
                if (Asset.DepreciationMethodId == null)
                {
                    return Ok(new { Message = "Please Select  Depreciation Method", status = false });
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
                if (Asset.Photo != string.Empty)
                {
                    var bytes = Convert.FromBase64String(Asset.Photo);
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/AssetPhotos");
                    string uniqePictureName = Guid.NewGuid().ToString("N") + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    Asset.Photo = uniqePictureName;
                }

                Asset.AssetStatusId = 1;
                _context.Assets.Add(Asset);
                string Str = "purchase Date : ";
                string AssetPurchaseDate = Asset.AssetPurchaseDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                AssetLog assetLog = new AssetLog()
                {
                    ActionLogId = 1,
                    Asset = Asset,
                    ActionDate = DateTime.Now,
                    Remark = string.Format($"{Str}{AssetPurchaseDate}")
                };
                _context.AssetLogs.Add(assetLog);
                try
                {
                    _context.SaveChanges();
                    return Ok(new { Message = "Asset Added successfully", status = true, Asset });

                }
                catch (Exception e)
                {
                    return Ok(new { Message = "Something went Error", status = false });

                }

            }
            return Ok(new { Message = "Data Not Valid", status = false });

        }

        [HttpPost]
        public async Task<IActionResult> EditAsset(Asset instance)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Asset = _context.Assets.FirstOrDefault(e => e.AssetId == instance.AssetId);
                    if (Asset == null)
                    {
                        return Ok(new { Message = "Something went Error", status = false });
                    }
                    if (instance.Photo != string.Empty &&instance.Photo!=Asset.Photo )
                    {
                        var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, Asset.Photo);
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                        var bytes = Convert.FromBase64String(instance.Photo);
                        string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/AssetPhotos");
                        string uniqePictureName = Guid.NewGuid().ToString("N") + ".jpeg";
                        string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                        using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        instance.Photo = uniqePictureName;
                    }
                    
                    if (instance.DepreciableAsset)
                    {
                        if (instance.DepreciationMethodId == null)
                        {
                            return Ok(new { Message = "Asset Not Edited,must select Depreciation Method", status = false });
                        }

                    }
                    else
                    {
                        instance.DepreciableCost = null;
                        instance.DateAcquired = null;
                        instance.DepreciationMethodId = null;
                        instance.SalvageValue = null;
                        instance.AssetLife = null;
                    }
                    var UpdatedAsset = _context.Assets.Attach(instance);
                    UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    AssetLog assetLog = new AssetLog()
                    {
                        ActionLogId = 19,
                        AssetId = instance.AssetId,
                        ActionDate = DateTime.Now,
                        Remark = string.Format("Asset Edited")
                    };
                    _context.AssetLogs.Add(assetLog);
                    _context.SaveChanges();
                    return Ok(new { Message = "Asset Edited successfully", status = true, instance });
                  
                }
                return Ok(new { Message = "Not Valid Data!", status = false});
               
            }
            catch (Exception e)
            {
                return Ok(new { Message = "Something went Error", status = false });
            }
        }

     

        [HttpPost]
        public async Task<IActionResult> AssetDispose([FromBody] List<AssetDisposeVm> assetIdsList)
        {
            List<DisposeAsset> Disposeasset = new List<DisposeAsset>();
            List<AssetLog> assetlogList = new List<AssetLog>();
            int count = 0;
            try
            {
               
                foreach (var item in assetIdsList)
                {
                    if (item.AssetId != 0)
                    {
                        var Assetobj = _context.Assets.Where(e => e.AssetId == item.AssetId).FirstOrDefault();
                        if (Assetobj == null)
                        {
                            return Ok(new { Message = $"Asset for AssetIdList for index No.= {count} not found ", status = false });
                        }
                        var dispose = _context.DisposeAssets.Where(e => e.DisposeAssetId == item.AssetId).FirstOrDefault();
                        if (dispose == null)
                        {
                            var disposeasset = new DisposeAsset()
                            {
                                DateDisposed = item.DateDisposed,
                                DisposeTo = item.DisposeTo,
                                Notes = item.Notes,

                            };
                            disposeasset.AssetDisposeDetails = new List<AssetDisposeDetails>();
                            disposeasset.AssetDisposeDetails.Add(new AssetDisposeDetails() { AssetId = item.AssetId, Remarks = "" });
                               Disposeasset.Add(disposeasset);

                            Assetobj.AssetStatusId = 5;
                            var UpdatedAsset = _context.Assets.Attach(Assetobj);
                            UpdatedAsset.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            string DisposeDate = "Dispose Date : ";
                            string DisposeTo = "Disposed To  : ";
                            string DisposeD = item.DateDisposed.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                            string DisposeFor = item.DisposeTo;
                            AssetLog assetLog = new AssetLog()
                            {
                                ActionLogId = 11,
                                AssetId = item.AssetId,
                                ActionDate = DateTime.Now,
                                Remark = string.Format($"{DisposeDate}{DisposeD} && {DisposeTo}{DisposeFor}")
                            };
                            assetlogList.Add(assetLog);
                        }
                    }

                }
                _context.AssetLogs.AddRange(assetlogList);
                _context.DisposeAssets.AddRange(Disposeasset);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return Ok(new { Message = e.Message, status = false });
            }
            return Ok(new { Message = "AssetDispose Added Successfully", status = true, disposeasset= Disposeasset });
        }
    }
}

