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
    public class ContractsController : Controller
    {
        private AssetContext _context;
        UserManager<ApplicationUser> UserManger;
        public Tenant tenant { set; get; }

        public ContractsController(AssetContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            tenant = _context.Tenants.Find(user.TenantId);
            var contracts = _context.Contracts.Include(e => e.tenant).Where(e => e.tenant == tenant).Select(i => new
            {
                i.ContractId,
                i.Title,
                i.Description,
                i.ContractNo,
                i.Cost,
                i.StartDate,
                i.EndDate,
                i.VendorId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ContractId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(contracts, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new Contract();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Contracts.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ContractId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.Contracts.FirstOrDefaultAsync(item => item.ContractId == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _context.Contracts.FirstOrDefaultAsync(item => item.ContractId == key);

            _context.Contracts.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Contract model, IDictionary values)
        {
            string CONTRACT_ID = nameof(Contract.ContractId);
            string TITLE = nameof(Contract.Title);
            string DESCRIPTION = nameof(Contract.Description);
            string CONTRACT_NO = nameof(Contract.ContractNo);
            string COST = nameof(Contract.Cost);
            string START_DATE = nameof(Contract.StartDate);
            string END_DATE = nameof(Contract.EndDate);
            string VENDOR_ID = nameof(Contract.VendorId);

            if (values.Contains(CONTRACT_ID))
            {
                model.ContractId = Convert.ToInt32(values[CONTRACT_ID]);
            }

            if (values.Contains(TITLE))
            {
                model.Title = Convert.ToString(values[TITLE]);
            }

            if (values.Contains(DESCRIPTION))
            {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if (values.Contains(CONTRACT_NO))
            {
                model.ContractNo = Convert.ToString(values[CONTRACT_NO]);
            }

            if (values.Contains(COST))
            {
                model.Cost = Convert.ToDouble(values[COST], CultureInfo.InvariantCulture);
            }

            if (values.Contains(START_DATE))
            {
                model.StartDate = Convert.ToDateTime(values[START_DATE]);
            }

            if (values.Contains(END_DATE))
            {
                model.EndDate = Convert.ToDateTime(values[END_DATE]);
            }

            if (values.Contains(VENDOR_ID))
            {
                model.VendorId = values[VENDOR_ID] != null ? Convert.ToInt32(values[VENDOR_ID]) : (int?)null;
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