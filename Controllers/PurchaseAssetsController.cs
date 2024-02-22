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

namespace AssetProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PurchaseAssetsController : Controller
    {
        private AssetContext _context;

        public PurchaseAssetsController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var purchaseassets = _context.PurchaseAssets.Select(i => new {
                i.PurchaseAssetId,
                i.PurchaseId,
                i.ItemId,
                i.Quantity,
                i.Price,
                i.Total,
                i.Discount,
                i.Net,
                i.Remarks,
                i.Item

            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PurchaseAssetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(purchaseassets, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PurchaseAsset();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PurchaseAssets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PurchaseAssetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PurchaseAssets.FirstOrDefaultAsync(item => item.PurchaseAssetId == key);
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
            var model = await _context.PurchaseAssets.FirstOrDefaultAsync(item => item.PurchaseAssetId == key);

            _context.PurchaseAssets.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> PurchasesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Purchases
                         orderby i.PurchaseSerial
                         select new {
                             Value = i.PurchaseId,
                             Text = i.PurchaseSerial
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ItemsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Items
                         orderby i.ItemTitle
                         select new {
                             Value = i.ItemId,
                             Text = i.ItemTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(PurchaseAsset model, IDictionary values) {
            string PURCHASE_ASSET_ID = nameof(PurchaseAsset.PurchaseAssetId);
            string PURCHASE_ID = nameof(PurchaseAsset.PurchaseId);
            string ITEM_ID = nameof(PurchaseAsset.ItemId);
            string QUANTITY = nameof(PurchaseAsset.Quantity);
            string PRICE = nameof(PurchaseAsset.Price);
            string TOTAL = nameof(PurchaseAsset.Total);
            string DISCOUNT = nameof(PurchaseAsset.Discount);
            string NET = nameof(PurchaseAsset.Net);
            string REMARKS = nameof(PurchaseAsset.Remarks);

            if(values.Contains(PURCHASE_ASSET_ID)) {
                model.PurchaseAssetId = Convert.ToInt32(values[PURCHASE_ASSET_ID]);
            }

            if(values.Contains(PURCHASE_ID)) {
                model.PurchaseId = Convert.ToInt32(values[PURCHASE_ID]);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if(values.Contains(QUANTITY)) {
                model.Quantity = values[QUANTITY] != null ? Convert.ToDouble(values[QUANTITY], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(PRICE)) {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(TOTAL)) {
                model.Total = values[TOTAL] != null ? Convert.ToDouble(values[TOTAL], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(DISCOUNT)) {
                model.Discount = values[DISCOUNT] != null ? Convert.ToDouble(values[DISCOUNT], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(NET)) {
                model.Net = values[NET] != null ? Convert.ToDouble(values[NET], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
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