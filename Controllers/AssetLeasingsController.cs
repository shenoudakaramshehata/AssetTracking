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
    public class AssetLeasingsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AssetLeasingsController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var assetleasings = _context.AssetLeasingDetails.Include(e => e.Asset).ThenInclude(e => e.tenant).Where(e => e.Asset.tenant == tenant).Select(
                i => new
                {
                    i.AssetLeasingDetailsId ,
                    i.AssetLeasingId,
                    i.Asset,
                    i.AssetId,
                    i.AssetLeasing,
                });


            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetLeasingId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetleasings, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetLeasing();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetLeasings.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetLeasingId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetLeasings.FirstOrDefaultAsync(item => item.AssetLeasingId == key);
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
            var model = await _context.AssetLeasings.FirstOrDefaultAsync(item => item.AssetLeasingId == key);

            _context.AssetLeasings.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CustomersLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Customers
                         orderby i.FullName
                         select new {
                             Value = i.CustomerId,
                             Text = i.FullName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(AssetLeasing model, IDictionary values) {
            string ASSET_LEASING_ID = nameof(AssetLeasing.AssetLeasingId);
            string START_DATE = nameof(AssetLeasing.StartDate);
            string END_DATE = nameof(AssetLeasing.EndDate);
            string NOTES = nameof(AssetLeasing.Notes);
            string CUSTOMER_ID = nameof(AssetLeasing.CustomerId);

            if(values.Contains(ASSET_LEASING_ID)) {
                model.AssetLeasingId = Convert.ToInt32(values[ASSET_LEASING_ID]);
            }

            if(values.Contains(START_DATE)) {
                model.StartDate = Convert.ToDateTime(values[START_DATE]);
            }

            if(values.Contains(END_DATE)) {
                model.EndDate = Convert.ToDateTime(values[END_DATE]);
            }

            if(values.Contains(NOTES)) {
                model.Notes = Convert.ToString(values[NOTES]);
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
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