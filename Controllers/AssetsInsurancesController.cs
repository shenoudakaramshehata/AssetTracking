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
    public class AssetsInsurancesController : Controller
    {
        private AssetContext _context;

        public AssetsInsurancesController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions, int AssetId) {
            var assetsinsurances = _context.AssetsInsurances.Where(e => e.AssetId == AssetId).Select(i => new {
                i.AssetsInsuranceId,
                i.AssetId,
                i.Insurance,i.InsuranceId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetsInsuranceId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetsinsurances, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetsInsurance();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetsInsurances.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetsInsuranceId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetsInsurances.FirstOrDefaultAsync(item => item.AssetsInsuranceId == key);
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
            var model = await _context.AssetsInsurances.FirstOrDefaultAsync(item => item.AssetsInsuranceId == key);

            _context.AssetsInsurances.Remove(model);
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

        [HttpGet]
        public async Task<IActionResult> InsurancesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Insurances
                         orderby i.Title
                         select new {
                             Value = i.InsuranceId,
                             Text = i.Title
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(AssetsInsurance model, IDictionary values) {
            string ASSETS_INSURANCE_ID = nameof(AssetsInsurance.AssetsInsuranceId);
            string ASSET_ID = nameof(AssetsInsurance.AssetId);
            string INSURANCE_ID = nameof(AssetsInsurance.InsuranceId);

            if(values.Contains(ASSETS_INSURANCE_ID)) {
                model.AssetsInsuranceId = Convert.ToInt32(values[ASSETS_INSURANCE_ID]);
            }

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
            }

            if(values.Contains(INSURANCE_ID)) {
                model.InsuranceId = Convert.ToInt32(values[INSURANCE_ID]);
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