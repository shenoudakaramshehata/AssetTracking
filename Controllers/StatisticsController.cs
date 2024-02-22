using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AssetProject.Data;
using AssetProject.Models;
using AssetProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssetProject.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StatisticsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public StatisticsController(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;

        }

        [HttpGet]
        public async Task <object> GetAssetCountsPerCategory(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.Categories.Where(e=>e.TenantId==tenant.TenantId).GroupBy(c => c.CategoryId).Select(g => new
            {
                Name = _context.Categories.FirstOrDefault(r => r.CategoryId == g.Key).CategoryTIAR,
                Count = _context.Assets.Include(e=>e.Item).Where(r => r.TenantId == tenant.TenantId && r.Item.CategoryId == g.Key).Count()

            }).OrderByDescending(r => r.Count);
            return listEn;
        }
        [HttpGet]
        public async Task<object> GetAssetCostByCategory(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.Categories.Where(e=>e.TenantId==tenant.TenantId).GroupBy(c => c.CategoryId).Select(g => new
            {
                Name = _context.Categories.FirstOrDefault(r => r.CategoryId == g.Key).CategoryTIAR,
                Cost = _context.Assets.Include(e=>e.Item).Where(r =>r.TenantId==tenant.TenantId &&r.Item.CategoryId == g.Key).Sum(s=>s.AssetCost)

            }).OrderByDescending(r => r.Cost);
            return listEn;
        }
        [HttpGet]
        public async Task<object> GetAssetCostByDepartment(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.Departments.Where(e=>e.TenantId==tenant.TenantId).GroupBy(c => c.DepartmentId).Select(g => new
            {
                Name = _context.Departments.FirstOrDefault(r => r.DepartmentId == g.Key).DepartmentTitle,
                Cost = _context.Assets.Include(a => a.AssetMovementDetails).ThenInclude(g=>g.AssetMovement)
                    .Where(a =>a.TenantId==tenant.TenantId&& a.AssetStatusId == 2 && a.AssetMovementDetails
                    .OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault()
                    .AssetMovement.DepartmentId == g.Key).Sum(a => a.AssetCost)

          }).OrderByDescending(r => r.Cost);
            return listEn;

            
        }
        public async Task<object> GetAssetCountByDepartmentAsync(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.Departments.Where(e=>e.TenantId==tenant.TenantId).GroupBy(c => c.DepartmentId).Select(g => new
            {
                Name = _context.Departments.FirstOrDefault(r => r.DepartmentId == g.Key).DepartmentTitle,
                Count = _context.Assets.Include(a => a.AssetMovementDetails).ThenInclude(g => g.AssetMovement)
        .Where(a =>a.TenantId==tenant.TenantId &&a.AssetStatusId == 2 && a.AssetMovementDetails
        .OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault()
        .AssetMovement.DepartmentId == g.Key).Count()

            }).OrderByDescending(r => r.Count);
            return listEn;


        }
        [HttpGet]
        public async Task<object> GetAssetCostByStatusAsync(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.AssetStatuses.GroupBy(c => c.AssetStatusId).Select(g => new
            {
                Name = _context.AssetStatuses.FirstOrDefault(r => r.AssetStatusId == g.Key).AssetStatusTitle,
                Cost = _context.Assets.Where(r =>r.TenantId==tenant.TenantId &&r.AssetStatusId== g.Key).Sum(s => s.AssetCost)

            }).OrderByDescending(r => r.Cost);



            return listEn;
        }
        [HttpGet]
        public async Task<object> GetAssetCountByStatusAsync(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.AssetStatuses.GroupBy(c => c.AssetStatusId).Select(g => new
            {
                Name = _context.AssetStatuses.FirstOrDefault(r => r.AssetStatusId == g.Key).AssetStatusTitle,
                Count = _context.Assets.Where(r =>r.TenantId==tenant.TenantId &&r.AssetStatusId == g.Key).Count()
            }).OrderByDescending(r => r.Count);



            return listEn;
        }
      
        public async Task<object> GetAssetCostByLocationAsync(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.Locations.Where(i=>i.TenantId==tenant.TenantId).GroupBy(c => c.LocationId).Select(g => new
            {
                Name = _context.Locations.FirstOrDefault(r => r.LocationId == g.Key).LocationTitle,
                Cost = _context.Assets.Include(a => a.AssetMovementDetails).ThenInclude(g => g.AssetMovement)
        .Where(a =>a.TenantId==tenant.TenantId &&a.AssetStatusId == 2 
         && a.AssetMovementDetails
        .OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault()
        .AssetMovement.LocationId == g.Key).Sum(a => a.AssetCost)

            }).OrderByDescending(r => r.Cost);
            return listEn;


        }
        public async Task<object> GetAssetCountByLocationAsync(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var listEn = _context.Locations.Where(i=>i.TenantId==tenant.TenantId).GroupBy(c => c.LocationId).Select(g => new
            {
                Name = _context.Locations.FirstOrDefault(r => r.LocationId == g.Key).LocationTitle,
                Count = _context.Assets.Include(a => a.AssetMovementDetails).ThenInclude(g => g.AssetMovement)
        .Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 2
         && a.AssetMovementDetails
        .OrderByDescending(a => a.AssetMovementDetailsId).FirstOrDefault()
        .AssetMovement.LocationId == g.Key).Count()

            }).OrderByDescending(r => r.Count);
            return listEn;


        }



    }
}