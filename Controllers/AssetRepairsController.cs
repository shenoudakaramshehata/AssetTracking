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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AssetProject.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AssetRepairsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public AssetRepairsController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var assetrepairs = _context.AssetRepairDetails.Include(e=>e.Asset).ThenInclude(e=>e.tenant).Where(e=>e.Asset.tenant==tenant).Select(
              i => new
              {
                  i.AssetRepairDetailsId,
                  i.AssetRepairId,
                  i.Asset,
                  i.AssetId,
                  i.AssetRepair
              });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetRepairId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(assetrepairs, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetRepair();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetRepairs.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetRepairId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetRepairs.FirstOrDefaultAsync(item => item.AssetRepairId == key);
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
            var model = await _context.AssetRepairs.FirstOrDefaultAsync(item => item.AssetRepairId == key);

            _context.AssetRepairs.Remove(model);
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
        public async Task<IActionResult> TechniciansLookup(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var lookup = from i in _context.Technicians
                         where i.TenantId==tenant.TenantId
                         orderby i.FullName
                         select new {
                             Value = i.TechnicianId,
                             Text = i.FullName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(AssetRepair model, IDictionary values) {
            string ASSET_REPAIR_ID = nameof(AssetRepair.AssetRepairId);
            string SCHEDULE_DATE = nameof(AssetRepair.ScheduleDate);
            string COMPLETED_DATE = nameof(AssetRepair.CompletedDate);
            string REPAIR_COST = nameof(AssetRepair.RepairCost);
            string NOTES = nameof(AssetRepair.Notes);
 
            string TECHNICIAN_ID = nameof(AssetRepair.TechnicianId);

            if(values.Contains(ASSET_REPAIR_ID)) {
                model.AssetRepairId = Convert.ToInt32(values[ASSET_REPAIR_ID]);
            }

            if(values.Contains(SCHEDULE_DATE)) {
                model.ScheduleDate = Convert.ToDateTime(values[SCHEDULE_DATE]);
            }

            if(values.Contains(COMPLETED_DATE)) {
                model.CompletedDate = Convert.ToDateTime(values[COMPLETED_DATE]);
            }

            if(values.Contains(REPAIR_COST)) {
                model.RepairCost = Convert.ToDouble(values[REPAIR_COST], CultureInfo.InvariantCulture);
            }

            if(values.Contains(NOTES)) {
                model.Notes = Convert.ToString(values[NOTES]);
            }


            if(values.Contains(TECHNICIAN_ID)) {
                model.TechnicianId = Convert.ToInt32(values[TECHNICIAN_ID]);
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