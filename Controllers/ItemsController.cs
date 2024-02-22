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
    public class ItemsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public ItemsController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var items = _context.Items.Include(e => e.tenant).Where(s => s.tenant == tenant).Select(i => new {
                i.ItemId,
                i.ItemTitle,
                i.CategoryId,
                i.BrandId,
                i.Description
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ItemId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Item();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Items.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ItemId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Items.FirstOrDefaultAsync(item => item.ItemId == key);
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
            var model = await _context.Items.FirstOrDefaultAsync(item => item.ItemId == key);

            _context.Items.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CategoriesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Categories
                         orderby i.CategoryTIAR
                         select new {
                             Value = i.CategoryId,
                             Text = i.CategoryTIAR
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> BrandsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Brands
                         orderby i.BrandTitle
                         select new {
                             Value = i.BrandId,
                             Text = i.BrandTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Item model, IDictionary values) {
            string ITEM_ID = nameof(Item.ItemId);
            string ITEM_TITLE = nameof(Item.ItemTitle);
            string CATEGORY_ID = nameof(Item.CategoryId);
            string BRAND_ID = nameof(Item.BrandId);
            string DESCRIPTION = nameof(Item.Description);

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if(values.Contains(ITEM_TITLE)) {
                model.ItemTitle = Convert.ToString(values[ITEM_TITLE]);
            }

            if(values.Contains(CATEGORY_ID)) {
                model.CategoryId = values[CATEGORY_ID] != null ? Convert.ToInt32(values[CATEGORY_ID]) : (int?)null;
            }

            if(values.Contains(BRAND_ID)) {
                model.BrandId = values[BRAND_ID] != null ? Convert.ToInt32(values[BRAND_ID]) : (int?)null;
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