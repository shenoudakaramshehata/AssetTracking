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
    public class AssetsManagementController : Controller
    {
        private AssetContext _context;

        public AssetsManagementController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var assets = _context.Assets.Select(i => new {
                i.AssetId,
                i.AssetDescription,
                i.AssetTagId,
                i.AssetCost,
                i.AssetSerialNo,
                i.AssetPurchaseDate,
                i.ItemId,
                i.Photo,
                i.DepreciableAsset,
                i.DepreciableCost,
                i.SalvageValue,
                i.AssetLife,
                i.DateAcquired,
                i.DepreciationMethodId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assets, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Asset();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Assets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Assets.FirstOrDefaultAsync(item => item.AssetId == key);
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
            var model = await _context.Assets.FirstOrDefaultAsync(item => item.AssetId == key);

            _context.Assets.Remove(model);
            await _context.SaveChangesAsync();
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

        [HttpGet]
        public async Task<IActionResult> DepreciationMethodsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.DepreciationMethods
                         orderby i.DepreciationMethodTitle
                         select new {
                             Value = i.DepreciationMethodId,
                             Text = i.DepreciationMethodTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Asset model, IDictionary values) {
            string ASSET_ID = nameof(Asset.AssetId);
            string ASSET_DESCRIPTION = nameof(Asset.AssetDescription);
            string ASSET_TAG_ID = nameof(Asset.AssetTagId);
            string ASSET_COST = nameof(Asset.AssetCost);
            string ASSET_SERIAL_NO = nameof(Asset.AssetSerialNo);
            string ASSET_PURCHASE_DATE = nameof(Asset.AssetPurchaseDate);
            string ITEM_ID = nameof(Asset.ItemId);
            string PHOTO = nameof(Asset.Photo);
            string DEPRECIABLE_ASSET = nameof(Asset.DepreciableAsset);
            string DEPRECIABLE_COST = nameof(Asset.DepreciableCost);
            string SALVAGE_VALUE = nameof(Asset.SalvageValue);
            string ASSET_LIFE = nameof(Asset.AssetLife);
            string DATE_ACQUIRED = nameof(Asset.DateAcquired);
            string DEPRECIATION_METHOD_ID = nameof(Asset.DepreciationMethodId);

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
            }

            if(values.Contains(ASSET_DESCRIPTION)) {
                model.AssetDescription = Convert.ToString(values[ASSET_DESCRIPTION]);
            }

            if(values.Contains(ASSET_TAG_ID)) {
                model.AssetTagId = Convert.ToString(values[ASSET_TAG_ID]);
            }

            if(values.Contains(ASSET_COST)) {
                model.AssetCost = Convert.ToDouble(values[ASSET_COST], CultureInfo.InvariantCulture);
            }

            if(values.Contains(ASSET_SERIAL_NO)) {
                model.AssetSerialNo = Convert.ToString(values[ASSET_SERIAL_NO]);
            }

            if(values.Contains(ASSET_PURCHASE_DATE)) {
                model.AssetPurchaseDate = Convert.ToDateTime(values[ASSET_PURCHASE_DATE]);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if(values.Contains(PHOTO)) {
                model.Photo = Convert.ToString(values[PHOTO]);
            }

            if(values.Contains(DEPRECIABLE_ASSET)) {
                model.DepreciableAsset = Convert.ToBoolean(values[DEPRECIABLE_ASSET]);
            }

            if(values.Contains(DEPRECIABLE_COST)) {
                model.DepreciableCost = values[DEPRECIABLE_COST] != null ? Convert.ToDouble(values[DEPRECIABLE_COST], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(SALVAGE_VALUE)) {
                model.SalvageValue = values[SALVAGE_VALUE] != null ? Convert.ToDouble(values[SALVAGE_VALUE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(ASSET_LIFE)) {
                model.AssetLife = values[ASSET_LIFE] != null ? Convert.ToInt32(values[ASSET_LIFE]) : (int?)null;
            }

            if(values.Contains(DATE_ACQUIRED)) {
                model.DateAcquired = values[DATE_ACQUIRED] != null ? Convert.ToDateTime(values[DATE_ACQUIRED]) : (DateTime?)null;
            }

            if(values.Contains(DEPRECIATION_METHOD_ID)) {
                model.DepreciationMethodId = values[DEPRECIATION_METHOD_ID] != null ? Convert.ToInt32(values[DEPRECIATION_METHOD_ID]) : (int?)null;
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