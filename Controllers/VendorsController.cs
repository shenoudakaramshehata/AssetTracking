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
    public class VendorsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }
        public VendorsController(AssetContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);

            var vendors = _context.Vendors.Include(i=>i.tenant).Where(i=>i.tenant==tenant).Select(i => new {
                i.VendorId,
                i.VendorTitle,
                i.Phone,
                i.Mobile,
                i.Email,
                i.Website,
                i.ContactPersonName,
                i.ContactPersonEmail,
                i.ContactPersonPhone
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "VendorId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(vendors, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Vendor();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Vendors.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.VendorId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Vendors.FirstOrDefaultAsync(item => item.VendorId == key);
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
            var model = await _context.Vendors.FirstOrDefaultAsync(item => item.VendorId == key);

            _context.Vendors.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Vendor model, IDictionary values) {
            string VENDOR_ID = nameof(Vendor.VendorId);
            string VENDOR_TITLE = nameof(Vendor.VendorTitle);
            string PHONE = nameof(Vendor.Phone);
            string MOBILE = nameof(Vendor.Mobile);
            string EMAIL = nameof(Vendor.Email);
            string WEBSITE = nameof(Vendor.Website);
            string CONTACT_PERSON_NAME = nameof(Vendor.ContactPersonName);
            string CONTACT_PERSON_EMAIL = nameof(Vendor.ContactPersonEmail);
            string CONTACT_PERSON_PHONE = nameof(Vendor.ContactPersonPhone);

            if(values.Contains(VENDOR_ID)) {
                model.VendorId = Convert.ToInt32(values[VENDOR_ID]);
            }

            if(values.Contains(VENDOR_TITLE)) {
                model.VendorTitle = Convert.ToString(values[VENDOR_TITLE]);
            }

            if(values.Contains(PHONE)) {
                model.Phone = Convert.ToString(values[PHONE]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(WEBSITE)) {
                model.Website = Convert.ToString(values[WEBSITE]);
            }

            if(values.Contains(CONTACT_PERSON_NAME)) {
                model.ContactPersonName = Convert.ToString(values[CONTACT_PERSON_NAME]);
            }

            if(values.Contains(CONTACT_PERSON_EMAIL)) {
                model.ContactPersonEmail = Convert.ToString(values[CONTACT_PERSON_EMAIL]);
            }

            if(values.Contains(CONTACT_PERSON_PHONE)) {
                model.ContactPersonPhone = Convert.ToString(values[CONTACT_PERSON_PHONE]);
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