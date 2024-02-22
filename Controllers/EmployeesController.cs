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
    public class EmployeesController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;


        public EmployeesController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        public Tenant tenant { set; get; }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var employees = _context.Employees.Include(e => e.tenant).Where(s =>s.tenant == tenant).Select(i => new {
                i.ID,
                i.EmployeeId,
                i.FullName,
                i.Title,
                i.Phone,
                i.Email,
                i.Notes,
                i.Remark
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "EmployeeId", "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(employees, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Employee();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Employees.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.EmployeeId, result.Entity.ID });
        }

        [HttpPut]
        public async Task<IActionResult> Put(string key, string values) {
            var keys = JsonConvert.DeserializeObject<IDictionary>(key);
            var keyEmployeeId = Convert.ToString(keys["EmployeeId"]);
            var keyID = Convert.ToInt32(keys["ID"]);
            var model = await _context.Employees.FirstOrDefaultAsync(item =>
                            item.EmployeeId == keyEmployeeId && 
                            item.ID == keyID);
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
        public async Task Delete(string key) {
            var keys = JsonConvert.DeserializeObject<IDictionary>(key);
            var keyEmployeeId = Convert.ToString(keys["EmployeeId"]);
            var keyID = Convert.ToInt32(keys["ID"]);
            var model = await _context.Employees.FirstOrDefaultAsync(item =>
                            item.EmployeeId == keyEmployeeId && 
                            item.ID == keyID);

            _context.Employees.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Employee model, IDictionary values) {
            string ID = nameof(Employee.ID);
            string EMPLOYEE_ID = nameof(Employee.EmployeeId);
            string FULL_NAME = nameof(Employee.FullName);
            string TITLE = nameof(Employee.Title);
            string PHONE = nameof(Employee.Phone);
            string EMAIL = nameof(Employee.Email);
            string NOTES = nameof(Employee.Notes);
            string REMARK = nameof(Employee.Remark);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(EMPLOYEE_ID)) {
                model.EmployeeId = Convert.ToString(values[EMPLOYEE_ID]);
            }

            if(values.Contains(FULL_NAME)) {
                model.FullName = Convert.ToString(values[FULL_NAME]);
            }

            if(values.Contains(TITLE)) {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if(values.Contains(PHONE)) {
                model.Phone = Convert.ToString(values[PHONE]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(NOTES)) {
                model.Notes = Convert.ToString(values[NOTES]);
            }

            if(values.Contains(REMARK)) {
                model.Remark = Convert.ToString(values[REMARK]);
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