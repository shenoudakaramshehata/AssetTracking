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
    public class CustomersController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public CustomersController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            var customers = _context.Customers.Include(i=>i.tenant).Where(i=>i.tenant==tenant).Select(i => new {
                i.CustomerId,
                i.FullName,
                i.CompanyName,
                i.Address1,
                i.Address2,
                i.City,
                i.State,
                i.PostalCode,
                i.Country,
                i.Phone,
                i.Email,
                i.Notes
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CustomerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(customers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Customer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CustomerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == key);
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
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == key);

            _context.Customers.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Customer model, IDictionary values) {
            string CUSTOMER_ID = nameof(Customer.CustomerId);
            string FULL_NAME = nameof(Customer.FullName);
            string COMPANY_NAME = nameof(Customer.CompanyName);
            string ADDRESS1 = nameof(Customer.Address1);
            string ADDRESS2 = nameof(Customer.Address2);
            string CITY = nameof(Customer.City);
            string STATE = nameof(Customer.State);
            string POSTAL_CODE = nameof(Customer.PostalCode);
            string COUNTRY = nameof(Customer.Country);
            string PHONE = nameof(Customer.Phone);
            string EMAIL = nameof(Customer.Email);
            string NOTES = nameof(Customer.Notes);

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(FULL_NAME)) {
                model.FullName = Convert.ToString(values[FULL_NAME]);
            }

            if(values.Contains(COMPANY_NAME)) {
                model.CompanyName = Convert.ToString(values[COMPANY_NAME]);
            }

            if(values.Contains(ADDRESS1)) {
                model.Address1 = Convert.ToString(values[ADDRESS1]);
            }

            if(values.Contains(ADDRESS2)) {
                model.Address2 = Convert.ToString(values[ADDRESS2]);
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

            if(values.Contains(COUNTRY)) {
                model.Country = Convert.ToString(values[COUNTRY]);
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