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
    public class InsurancesController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public InsurancesController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            var insurances = _context.Insurances.Include(i=>i.tenant).Where(i=>i.tenant==tenant).Select(i => new {
                i.InsuranceId,
                i.Title,
                i.Description,
                i.InsuranceCompany,
                i.ContactPerson,
                i.PolicyNo,
                i.Phone,
                i.StartDate,
                i.EndDate,
                i.Deductible,
                i.Permium,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "InsuranceId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(insurances, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Insurance();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Insurances.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.InsuranceId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Insurances.FirstOrDefaultAsync(item => item.InsuranceId == key);
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
            var model = await _context.Insurances.FirstOrDefaultAsync(item => item.InsuranceId == key);

            _context.Insurances.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Insurance model, IDictionary values) {
            string INSURANCE_ID = nameof(Insurance.InsuranceId);
            string TITLE = nameof(Insurance.Title);
            string DESCRIPTION = nameof(Insurance.Description);
            string INSURANCE_COMPANY = nameof(Insurance.InsuranceCompany);
            string CONTACT_PERSON = nameof(Insurance.ContactPerson);
            string POLICY_NO = nameof(Insurance.PolicyNo);
            string PHONE = nameof(Insurance.Phone);
            string START_DATE = nameof(Insurance.StartDate);
            string END_DATE = nameof(Insurance.EndDate);
            string DEDUCTIBLE = nameof(Insurance.Deductible);
            string PERMIUM = nameof(Insurance.Permium);
            string IS_ACTIVE = nameof(Insurance.IsActive);

            if(values.Contains(INSURANCE_ID)) {
                model.InsuranceId = Convert.ToInt32(values[INSURANCE_ID]);
            }

            if(values.Contains(TITLE)) {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(INSURANCE_COMPANY)) {
                model.InsuranceCompany = Convert.ToString(values[INSURANCE_COMPANY]);
            }

            if(values.Contains(CONTACT_PERSON)) {
                model.ContactPerson = Convert.ToString(values[CONTACT_PERSON]);
            }

            if(values.Contains(POLICY_NO)) {
                model.PolicyNo = Convert.ToString(values[POLICY_NO]);
            }

            if(values.Contains(PHONE)) {
                model.Phone = Convert.ToString(values[PHONE]);
            }

            if(values.Contains(START_DATE)) {
                model.StartDate = Convert.ToDateTime(values[START_DATE]);
            }

            if(values.Contains(END_DATE)) {
                model.EndDate = Convert.ToDateTime(values[END_DATE]);
            }

            if(values.Contains(DEDUCTIBLE)) {
                model.Deductible = Convert.ToDecimal(values[DEDUCTIBLE], CultureInfo.InvariantCulture);
            }

            if(values.Contains(PERMIUM)) {
                model.Permium = Convert.ToDecimal(values[PERMIUM], CultureInfo.InvariantCulture);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = Convert.ToBoolean(values[IS_ACTIVE]);
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