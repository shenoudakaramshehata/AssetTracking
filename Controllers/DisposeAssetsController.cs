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
    public class DisposeAssetsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public DisposeAssetsController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var disposeassets = _context.AssetDisposeDetails.Include(e => e.Asset).ThenInclude(e => e.tenant).Where(e => e.Asset.tenant == tenant).Select(
                i => new
                {
                    i.AssetDisposeDetailsId,
                    i.DisposeAssetId,
                    i.Asset,
                    i.AssetId,
                    i.DisposeAsset
                });


            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "DisposeAssetId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(disposeassets, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new DisposeAsset();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.DisposeAssets.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.DisposeAssetId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.DisposeAssets.FirstOrDefaultAsync(item => item.DisposeAssetId == key);
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
            var model = await _context.DisposeAssets.FirstOrDefaultAsync(item => item.DisposeAssetId == key);

            _context.DisposeAssets.Remove(model);
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

        private void PopulateModel(DisposeAsset model, IDictionary values) {
            string DISPOSE_ASSET_ID = nameof(DisposeAsset.DisposeAssetId);
        
            string DATE_DISPOSED = nameof(DisposeAsset.DateDisposed);
            string DISPOSE_TO = nameof(DisposeAsset.DisposeTo);
            string NOTES = nameof(DisposeAsset.Notes);

            if(values.Contains(DISPOSE_ASSET_ID)) {
                model.DisposeAssetId = Convert.ToInt32(values[DISPOSE_ASSET_ID]);
            }


            if(values.Contains(DATE_DISPOSED)) {
                model.DateDisposed = Convert.ToDateTime(values[DATE_DISPOSED]);
            }

            if(values.Contains(DISPOSE_TO)) {
                model.DisposeTo = Convert.ToString(values[DISPOSE_TO]);
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