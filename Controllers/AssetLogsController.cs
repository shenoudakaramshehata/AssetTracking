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
    public class AssetLogsController : Controller
    {
        private AssetContext _context;

        public AssetLogsController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions,int AssetId) {
            var assetlogs = _context.AssetLogs.Where(e=>e.AssetId==AssetId).Select(i => new {
                i.AssetLogId,
                i.ActionDate,
                i.Remark,
                i.AssetId,
                i.ActionLogId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetLogId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetlogs, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetLog();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetLogs.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetLogId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetLogs.FirstOrDefaultAsync(item => item.AssetLogId == key);
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
            var model = await _context.AssetLogs.FirstOrDefaultAsync(item => item.AssetLogId == key);

            _context.AssetLogs.Remove(model);
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
        public async Task<IActionResult> ActionLogsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.ActionLogs
                         orderby i.ActionLogTitle
                         select new {
                             Value = i.ActionLogId,
                             Text = i.ActionLogTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(AssetLog model, IDictionary values) {
            string ASSET_LOG_ID = nameof(AssetLog.AssetLogId);
            string ACTION_DATE = nameof(AssetLog.ActionDate);
            string REMARK = nameof(AssetLog.Remark);
            string ASSET_ID = nameof(AssetLog.AssetId);
            string ACTION_LOG_ID = nameof(AssetLog.ActionLogId);

            if(values.Contains(ASSET_LOG_ID)) {
                model.AssetLogId = Convert.ToInt32(values[ASSET_LOG_ID]);
            }

            if(values.Contains(ACTION_DATE)) {
                model.ActionDate = Convert.ToDateTime(values[ACTION_DATE]);
            }

            if(values.Contains(REMARK)) {
                model.Remark = Convert.ToString(values[REMARK]);
            }

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
            }

            if(values.Contains(ACTION_LOG_ID)) {
                model.ActionLogId = Convert.ToInt32(values[ACTION_LOG_ID]);
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