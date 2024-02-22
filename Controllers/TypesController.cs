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
using Type = AssetProject.Models.Type;

namespace AssetProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TypesController : Controller
    {
        private AssetContext _context;

        public TypesController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var types = _context.Types.Select(i => new {
                i.TypeId,
                i.TypeTitle,
                i.BrandId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "TypeId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(types, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Type();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Types.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.TypeId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Types.FirstOrDefaultAsync(item => item.TypeId == key);
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
            var model = await _context.Types.FirstOrDefaultAsync(item => item.TypeId == key);

            _context.Types.Remove(model);
            await _context.SaveChangesAsync();
        }


       
        private void PopulateModel(Type model, IDictionary values) {
            string TYPE_ID = nameof(Type.TypeId);
            string TYPE_TITLE = nameof(Type.TypeTitle);
            string BRAND_ID = nameof(Type.BrandId);

            if(values.Contains(TYPE_ID)) {
                model.TypeId = Convert.ToInt32(values[TYPE_ID]);
            }

            if(values.Contains(TYPE_TITLE)) {
                model.TypeTitle = Convert.ToString(values[TYPE_TITLE]);
            }

            if(values.Contains(BRAND_ID)) {
                model.BrandId = values[BRAND_ID] != null ? Convert.ToInt32(values[BRAND_ID]) : (int?)null;
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