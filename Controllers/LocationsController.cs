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
    public class LocationsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public LocationsController(AssetContext context,UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;

        }

        
        public Tenant tenant { set; get; }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
                var locations = _context.Locations.Include(e=>e.tenant).Where(e => e.tenant == tenant).Select(i => new {
                    i.LocationId,
                    i.LocationParentId,
                    i.LocationTitle,
                    i.CountryId,
                    i.Address,
                    i.City,
                    i.State,
                    i.PostalCode,
                    i.BarCode,
                    i.LocationLangtiude,
                    i.LocationLatitude
                });

                // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
                // This can make SQL execution plans more efficient.
                // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
                // loadOptions.PrimaryKey = new[] { "LocationId" };
                // loadOptions.PaginateViaPrimaryKey = true;

                return Json(await DataSourceLoader.LoadAsync(locations, loadOptions));
            
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            var model = new Location() { TenantId=tenant.TenantId};
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Locations.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.LocationId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Locations.FirstOrDefaultAsync(item => item.LocationId == key);
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
            var model = await _context.Locations.FirstOrDefaultAsync(item => item.LocationId == key);

            _context.Locations.Remove(model);
            await _context.SaveChangesAsync();
        }


    

        private void PopulateModel(Location model, IDictionary values) {
            string LOCATION_ID = nameof(Location.LocationId);
            string LOCATION_PARENT_ID = nameof(Location.LocationParentId);
            string LOCATION_TITLE = nameof(Location.LocationTitle);
            string COUNTRY_ID = nameof(Location.CountryId);
            string ADDRESS = nameof(Location.Address);
            string CITY = nameof(Location.City);
            string STATE = nameof(Location.State);
            string POSTAL_CODE = nameof(Location.PostalCode);
            string BARCODE = nameof(Location.BarCode);
            string Langtude = nameof(Location.LocationLangtiude);
            string Latetude = nameof(Location.LocationLatitude);
            
            if (values.Contains(LOCATION_ID)) {
                model.LocationId = Convert.ToInt32(values[LOCATION_ID]);
            }

            if(values.Contains(LOCATION_PARENT_ID)) {
                model.LocationParentId = values[LOCATION_PARENT_ID] != null ? Convert.ToInt32(values[LOCATION_PARENT_ID]) : (int?)null;
            }

            if(values.Contains(LOCATION_TITLE)) {
                model.LocationTitle = Convert.ToString(values[LOCATION_TITLE]);
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = values[COUNTRY_ID] != null ? Convert.ToInt32(values[COUNTRY_ID]) : (int?)null;
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(CITY)) {
                model.City = Convert.ToString(values[CITY]);
            }

            if(values.Contains(STATE)) {
                model.State = Convert.ToString(values[STATE]);
            }

            if(values.Contains(POSTAL_CODE)) {
                model.PostalCode = Convert.ToString(values[POSTAL_CODE]);
            }

            if (values.Contains(BARCODE))
            {
                model.BarCode = Convert.ToString(values[BARCODE]);
            }
            if (values.Contains(Langtude))
            {
                model.LocationLangtiude = Convert.ToString(values[Langtude]);
            }
            if (values.Contains(Latetude))
            {
                model.LocationLatitude = Convert.ToString(values[Latetude]);
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