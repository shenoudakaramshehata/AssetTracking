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
    public class DepartmentsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;

        public DepartmentsController(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        public Tenant tenant { set; get; }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var departments = _context.Departments.Include(e => e.tenant).Where(e => e.tenant == tenant).Select(i => new {
                i.DepartmentId,
                i.DepartmentTitle
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "DepartmentId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(departments, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var model = new Department(){ TenantId = tenant.TenantId };
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Departments.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.DepartmentId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Departments.FirstOrDefaultAsync(item => item.DepartmentId == key);
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
            var model = await _context.Departments.FirstOrDefaultAsync(item => item.DepartmentId == key);

            _context.Departments.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Department model, IDictionary values) {
            string DEPARTMENT_ID = nameof(Department.DepartmentId);
            string DEPARTMENT_TITLE = nameof(Department.DepartmentTitle);

            if(values.Contains(DEPARTMENT_ID)) {
                model.DepartmentId = Convert.ToInt32(values[DEPARTMENT_ID]);
            }

            if(values.Contains(DEPARTMENT_TITLE)) {
                model.DepartmentTitle = Convert.ToString(values[DEPARTMENT_TITLE]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
       

    }
}