using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.Dashboards
{
    [Authorize]
    public class DashboardSummaryModel : PageModel
    {

        private readonly AssetContext _context;
        public int TotalAssetCount { get; set; }
        public double TotalAssetValue { get; set; }
        public int TotalAssetAvaliable { get; set; }
        public int TotalAssetActive { get; set; }
        public int TotalAssetBrocken { get; set; }
        public double TotalAssetBrockenValue { get; set; }
        public double TotalSellAsst { get; set; }
        public double TotalSellAsstValue { get; set; }
        public int TotalAssetUnderRepair { get; set; }
        public double TotalAssetUnderRepairCost { get; set; }
        public int TotalAssetLeased { get; set; }
        public double TotalLeasedAssetCost = 0;
        public int TotalAssetLost { get; set; }
        public double TotalAssetLostCost { get; set; }
        public int TotalAssetDispose { get; set; }
        public double TotalAssetDisposeCost { get; set; }
        public int TotalAssetMaint { get; set; }
        public double TotalAssetMaintCost { get; set; }
        public int TotalAssetCheckOut { get; set; }
        public double TotalAssetCheckOutCost { get; set; }
        public int TotalAssetLinkInsurance { get; set; }
        public int TotalAssetLinkWarrenty { get; set; }
        public int TotalAssetLinkContract { get; set; }
        public int TotalAssetPurchaseCY { get; set; }
        public double TotalAssetPurchaseCostCY { get; set; }

        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public DashboardSummaryModel(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        public async Task <IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            TotalAssetCount = _context.Assets.Where(e=>e.TenantId==tenant.TenantId).Count();
            TotalAssetValue = _context.Assets.Where(e => e.TenantId == tenant.TenantId).Sum(a=>a.AssetCost);
            TotalAssetActive = _context.Assets.Where(a =>a.TenantId==tenant.TenantId &&(a.AssetStatusId == 1|| a.AssetStatusId==2|| a.AssetStatusId==3|| a.AssetStatusId==9)).Count();
            TotalAssetAvaliable = _context.Assets.Where(a =>a.TenantId==tenant.TenantId &&a.AssetStatusId ==1).Count();
            TotalAssetBrocken = _context.Assets.Where(a =>a.TenantId==tenant.TenantId &&a.AssetStatusId ==8).Count();
            TotalAssetBrockenValue = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 8).Sum(a => a.AssetCost);
            TotalSellAsst = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 7).Count();
            TotalSellAsstValue = _context.AssetSellDetails.Where(e=>e.Asset.TenantId==tenant.TenantId).Sum(a => a.SellAsset.SaleAmount);
            TotalAssetUnderRepair = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 3).Count();
            var listmaxassetrepairId =
                 from a in _context.Assets
                 where a.TenantId == tenant.TenantId&&a.AssetStatusId == 3
                 from r in _context.AssetRepairs
                 from rd in _context.AssetRepairDetails
                 where a.AssetId == rd.AssetId && r.AssetRepairId == rd.AssetRepairId
                 group rd by rd.AssetId
                 into gr
                 select new
                 {
                     AMIDS = (from AMD in gr select AMD.AssetRepairId).Max()
                 };

            TotalAssetUnderRepairCost = 0;
            foreach (var item in listmaxassetrepairId)
            {
                var costofmaxid = _context.AssetRepairs.Where(i => i.AssetRepairId == item.AMIDS).Select(e => e.RepairCost).FirstOrDefault();
                TotalAssetUnderRepairCost += costofmaxid;
            }
            var listmaxassetLeasingId =
                 from a in _context.Assets
                 where a.TenantId==tenant.TenantId&&a.AssetStatusId == 6
                 from r in _context.AssetLeasings
                 from rd in _context.AssetLeasingDetails
                 where a.AssetId == rd.AssetId && r.AssetLeasingId == rd.AssetLeasingId
                 group rd by rd.AssetId
                 into gr
                 select new
                 {
                     AMIDS = (from AMD in gr select AMD.AssetLeasingId).Max()
                 };
             
            foreach (var item in listmaxassetLeasingId)
            {
                var costofmaxid = _context.AssetLeasings.Where(i => i.AssetLeasingId == item.AMIDS).Select(e => e.LeasedCost).FirstOrDefault();
                TotalLeasedAssetCost += costofmaxid;
            }
            TotalAssetMaint = _context.AssetMaintainances.Where(m =>m.Asset.TenantId==tenant.TenantId&&m.MaintainanceStatusId != 4 && m.MaintainanceStatusId != 5).Count();
            TotalAssetMaintCost = _context.AssetMaintainances.Where(m =>m.Asset.TenantId==tenant.TenantId&& m.MaintainanceStatusId != 4 && m.MaintainanceStatusId != 5).Sum(e=>e.AssetMaintainanceRepairesCost);
            TotalAssetLeased = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 6).Count();
            TotalAssetLost = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 4).Count();
            TotalAssetLostCost = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 4).Sum(a=>a.AssetCost);
            TotalAssetDispose = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 5).Count();
            TotalAssetDisposeCost = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 5).Sum(a=>a.AssetCost);
            TotalAssetCheckOut = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 2).Count();
            TotalAssetCheckOutCost = _context.Assets.Where(a => a.TenantId == tenant.TenantId && a.AssetStatusId == 2).Sum(a => a.AssetCost);
           
            var AssetIdWithinsurance = (from c in _context.AssetsInsurances
                                        where c.Asset.TenantId==tenant.TenantId
                                        orderby c.AssetId
                                        select c.AssetId).Distinct();

            double AssetLinkInsuranceCost = 0;
            foreach (var item in AssetIdWithinsurance)
            {
                AssetLinkInsuranceCost += _context.Assets.Where(a => a.AssetId == item).Sum(a => a.AssetCost);
            }

            TotalAssetLinkInsurance = (from IN in
               _context.AssetsInsurances
                                       where IN.Asset.TenantId == tenant.TenantId
                                       orderby IN.AssetId
                                       select IN.AssetId).Distinct().Count();

            TotalAssetLinkWarrenty = (from W in
               _context.AssetWarranties
                                      where W.Asset.TenantId == tenant.TenantId
                                      orderby W.AssetId
                                      select W.AssetId).Distinct().Count();
           
            TotalAssetLinkContract = (from C in
               _context.AssetContracts
                                      where C.Asset.TenantId == tenant.TenantId
                                      orderby C.AssetId
                                      select C.AssetId).Distinct().Count();
            TotalAssetPurchaseCY = _context.Assets.Where(A=>A.TenantId==tenant.TenantId&&A.AssetPurchaseDate.Date.Year==DateTime.Now.Year).Count();
            TotalAssetPurchaseCostCY = _context.Assets.Where(A=>A.TenantId==tenant.TenantId&&A.AssetPurchaseDate.Date.Year==DateTime.Now.Year).Sum(a=>a.AssetCost);

            return Page();
        }
    }
}
