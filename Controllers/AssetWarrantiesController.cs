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
    public class AssetWarrantiesController : Controller
    {
        private AssetContext _context;

        public AssetWarrantiesController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions,int AssetId) {
            var assetwarranties = _context.AssetWarranties.Where(a=>a.AssetId== AssetId).Select(i => new {
                i.WarrantyId,
                i.Length,
                i.ExpirationDate,
                i.Notes,
                i.AssetId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "WarrantyId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetwarranties, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetWarranty();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetWarranties.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.WarrantyId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetWarranties.FirstOrDefaultAsync(item => item.WarrantyId == key);
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
            var model = await _context.AssetWarranties.FirstOrDefaultAsync(item => item.WarrantyId == key);

            _context.AssetWarranties.Remove(model);
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

        private void PopulateModel(AssetWarranty model, IDictionary values) {
            string WARRANTY_ID = nameof(AssetWarranty.WarrantyId);
            string LENGTH = nameof(AssetWarranty.Length);
            string EXPIRATION_DATE = nameof(AssetWarranty.ExpirationDate);
            string NOTES = nameof(AssetWarranty.Notes);
            string ASSET_ID = nameof(AssetWarranty.AssetId);

            if(values.Contains(WARRANTY_ID)) {
                model.WarrantyId = Convert.ToInt32(values[WARRANTY_ID]);
            }

            if(values.Contains(LENGTH)) {
                model.Length = Convert.ToInt32(values[LENGTH]);
            }

            if(values.Contains(EXPIRATION_DATE)) {
                model.ExpirationDate = Convert.ToDateTime(values[EXPIRATION_DATE]);
            }

            if(values.Contains(NOTES)) {
                model.Notes = Convert.ToString(values[NOTES]);
            }

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
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