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
    public class AssetDocumentsController : Controller
    {
        private AssetContext _context;

        public AssetDocumentsController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions, int AssetId) {
           
            var assetdocuments = _context.assetDocuments .Where(e=>e.AssetId==AssetId).Select(i => new
            {
                i.AssetDocumentId,
                i.AssetId,
                i.DocumentName,
                i.DocumentType,
                i.Description
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetDocumentId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetdocuments, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetDocument();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.assetDocuments.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetDocumentId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.assetDocuments.FirstOrDefaultAsync(item => item.AssetDocumentId == key);
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
            var model = await _context.assetDocuments.FirstOrDefaultAsync(item => item.AssetDocumentId == key);

            _context.assetDocuments.Remove(model);
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

        private void PopulateModel(AssetDocument model, IDictionary values) {
            string ASSET_DOCUMENT_ID = nameof(AssetDocument.AssetDocumentId);
            string ASSET_ID = nameof(AssetDocument.AssetId);
            string DOCUMENT_NAME = nameof(AssetDocument.DocumentName);
            string DOCUMENT_TYPE = nameof(AssetDocument.DocumentType);
            string DESCRIPTION = nameof(AssetDocument.Description);

            if(values.Contains(ASSET_DOCUMENT_ID)) {
                model.AssetDocumentId = Convert.ToInt32(values[ASSET_DOCUMENT_ID]);
            }

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
            }

            if(values.Contains(DOCUMENT_NAME)) {
                model.DocumentName = Convert.ToString(values[DOCUMENT_NAME]);
            }

            if(values.Contains(DOCUMENT_TYPE)) {
                model.DocumentType = Convert.ToString(values[DOCUMENT_TYPE]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
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