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
    public class TechniciansController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public TechniciansController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var technicians = _context.Technicians.Include(e => e.Tenant).Where(e => e.Tenant == tenant).Select(i => new {
                i.TechnicianId,
                i.FullName,
                i.Mobile,
                i.Address,
                i.Remarks
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "TechnicianId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(technicians, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Technician();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Technicians.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.TechnicianId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Technicians.FirstOrDefaultAsync(item => item.TechnicianId == key);
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
            var model = await _context.Technicians.FirstOrDefaultAsync(item => item.TechnicianId == key);

            _context.Technicians.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> TenantsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Tenants
                         orderby i.CompanyName
                         select new {
                             Value = i.TenantId,
                             Text = i.CompanyName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Technician model, IDictionary values) {
            string TECHNICIAN_ID = nameof(Technician.TechnicianId);
            string FULL_NAME = nameof(Technician.FullName);
            string MOBILE = nameof(Technician.Mobile);
            string ADDRESS = nameof(Technician.Address);
            string REMARKS = nameof(Technician.Remarks);
            string TENANT_ID = nameof(Technician.TenantId);

            if(values.Contains(TECHNICIAN_ID)) {
                model.TechnicianId = Convert.ToInt32(values[TECHNICIAN_ID]);
            }

            if(values.Contains(FULL_NAME)) {
                model.FullName = Convert.ToString(values[FULL_NAME]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if(values.Contains(TENANT_ID)) {
                model.TenantId = values[TENANT_ID] != null ? Convert.ToInt32(values[TENANT_ID]) : (int?)null;
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