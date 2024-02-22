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
    public class AssetContractsController : Controller
    {
        private AssetContext _context;

        public AssetContractsController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions,int AssetId) {

            var assetcontracts = _context.AssetContracts.Where(e=>e.AssetId==AssetId).Select(i => new {
                i.AssetContractID,
                i.Contract,
                i.ContractId,
            });


            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetContractID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetcontracts, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetContract();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetContracts.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetContractID });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetContracts.FirstOrDefaultAsync(item => item.AssetContractID == key);
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
            var model = await _context.AssetContracts.FirstOrDefaultAsync(item => item.AssetContractID == key);

            _context.AssetContracts.Remove(model);
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
        public async Task<IActionResult> ContractsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Contracts
                         orderby i.Title
                         select new {
                             Value = i.ContractId,
                             Text = i.Title
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(AssetContract model, IDictionary values) {
            string ASSET_CONTRACT_ID = nameof(AssetContract.AssetContractID);
            string ASSET_ID = nameof(AssetContract.AssetId);
            string CONTRACT_ID = nameof(AssetContract.ContractId);

            if(values.Contains(ASSET_CONTRACT_ID)) {
                model.AssetContractID = Convert.ToInt32(values[ASSET_CONTRACT_ID]);
            }

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
            }

            if (values.Contains(CONTRACT_ID))
            {
                model.ContractId = values[CONTRACT_ID] != null ? Convert.ToInt32(values[CONTRACT_ID]) : (int?)null;
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