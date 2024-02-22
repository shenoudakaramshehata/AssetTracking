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
    public class SubCategoriesController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;


        public SubCategoriesController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;

        }
        public Tenant tenant { set; get; }


        [HttpGet]
        public async Task<IActionResult> Get(int CategoryId, DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var subcategories = _context.SubCategories.Include(e=>e.tenant).Where(s=>s.CategoryId==CategoryId && s.tenant==tenant).Select(i => new {
                i.SubCategoryId,
                i.SubCategoryTitle,
                i.SubCategoryDescription,
                i.CategoryId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "SubCategoryId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(subcategories, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post( string values) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var model = new SubCategory() { TenantId = tenant.TenantId };
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.SubCategories.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.SubCategoryId });
        }

        [HttpPut]
        public async Task<IActionResult> Put( int key, string values) {
            var model = await _context.SubCategories.FirstOrDefaultAsync(item => item.SubCategoryId == key);
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
            var model = await _context.SubCategories.FirstOrDefaultAsync(item => item.SubCategoryId == key);

            _context.SubCategories.Remove(model);
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

        private void PopulateModel(SubCategory model, IDictionary values) {
            string SUB_CATEGORY_ID = nameof(SubCategory.SubCategoryId);
            string SUB_CATEGORY_TITLE = nameof(SubCategory.SubCategoryTitle);
            string SUB_CATEGORY_DESCRIPTION = nameof(SubCategory.SubCategoryDescription);
            string CATEGORY_ID = nameof(SubCategory.CategoryId);

            if(values.Contains(SUB_CATEGORY_ID)) {
                model.SubCategoryId = Convert.ToInt32(values[SUB_CATEGORY_ID]);
            }

            if(values.Contains(SUB_CATEGORY_TITLE)) {
                model.SubCategoryTitle = Convert.ToString(values[SUB_CATEGORY_TITLE]);
            }

            if(values.Contains(SUB_CATEGORY_DESCRIPTION)) {
                model.SubCategoryDescription = Convert.ToString(values[SUB_CATEGORY_DESCRIPTION]);
            }
            //model.CategoryId = CategoryId;

            if (values.Contains(CATEGORY_ID))
            {
                model.CategoryId = Convert.ToInt32(values[CATEGORY_ID]);
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