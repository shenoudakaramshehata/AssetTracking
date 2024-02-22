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
    public class StoresController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public StoresController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;

        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var stores = _context.Stores.Include(e => e.tenant).Where(e => e.tenant == tenant).Select(i => new {
                i.StoreId,
                i.StoreTitle,
                i.Address,
                i.Tele,
                i.Mobile,
                i.Fax
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "StoreId" };
            //  loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(stores, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Store();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Stores.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.StoreId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Stores.FirstOrDefaultAsync(item => item.StoreId == key);
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
            var model = await _context.Stores.FirstOrDefaultAsync(item => item.StoreId == key);

            _context.Stores.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Store model, IDictionary values) {
            string STORE_ID = nameof(Store.StoreId);
            string STORE_TITLE = nameof(Store.StoreTitle);
            string ADRESS = nameof(Store.Address);
            string TELE = nameof(Store.Tele);
            string MOBILE = nameof(Store.Mobile);
            string FAX = nameof(Store.Fax);

            if(values.Contains(STORE_ID)) {
                model.StoreId = Convert.ToInt32(values[STORE_ID]);
            }

            if(values.Contains(STORE_TITLE)) {
                model.StoreTitle = Convert.ToString(values[STORE_TITLE]);
            }

            if(values.Contains(ADRESS)) {
                model.Address = Convert.ToString(values[ADRESS]);
            }

            if(values.Contains(TELE)) {
                model.Tele = Convert.ToString(values[TELE]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(FAX)) {
                model.Fax = Convert.ToString(values[FAX]);
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