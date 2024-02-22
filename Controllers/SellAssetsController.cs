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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssetProject.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SellAssetsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public SellAssetsController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            var sellassets = _context.AssetSellDetails.Include(e => e.Asset).ThenInclude(e => e.tenant).Where(e => e.Asset.tenant == tenant).Select(
               i => new
               {
                   i.AssetSellDetailsId,
                   i.SellAssetId,
                   i.Asset,
                   i.AssetId,
                   i.SellAsset
               });


            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "SellAssetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(sellassets, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new SellAsset();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.sellAssets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.SellAssetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.sellAssets.FirstOrDefaultAsync(item => item.SellAssetId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.sellAssets.FirstOrDefaultAsync(item => item.SellAssetId == key);

            _context.sellAssets.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> AssetsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Assets
                         orderby i.AssetDescription
                         select new {
                             Value = i.AssetId,
                             Text = i.AssetDescription
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(SellAsset model, IDictionary values) {
            string SELL_ASSET_ID = nameof(SellAsset.SellAssetId);
            string SALE_DATE = nameof(SellAsset.SaleDate);
            string SOLD_TO = nameof(SellAsset.SoldTo);
            string SALE_AMOUNT = nameof(SellAsset.SaleAmount);
            string NOTES = nameof(SellAsset.Notes);


            if(values.Contains(SELL_ASSET_ID)) {
                model.SellAssetId = Convert.ToInt32(values[SELL_ASSET_ID]);
            }

            if(values.Contains(SALE_DATE)) {
                model.SaleDate = Convert.ToDateTime(values[SALE_DATE]);
            }

            if(values.Contains(SOLD_TO)) {
                model.SoldTo = Convert.ToString(values[SOLD_TO]);
            }

            if(values.Contains(SALE_AMOUNT)) {
                model.SaleAmount = Convert.ToDouble(values[SALE_AMOUNT], CultureInfo.InvariantCulture);
            }

            if(values.Contains(NOTES)) {
                model.Notes = Convert.ToString(values[NOTES]);
            }

    
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}