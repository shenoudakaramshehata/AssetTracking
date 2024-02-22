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

namespace AssetProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PurchasesController : Controller
    {
        private AssetContext _context;

        public PurchasesController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var purchases = _context.Purchases.Select(i => new {
                i.PurchaseId,
                i.PurchaseSerial,
                i.Purchasedate,
                i.StoreId,
                i.VendorId,
                i.Total,
                i.Discount,
                i.Net,
                i.Remarks,
                i.PurchaseAssets
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PurchaseId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(purchases, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Purchase();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Purchases.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PurchaseId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Purchases.FirstOrDefaultAsync(item => item.PurchaseId == key);
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
            var model = await _context.Purchases.FirstOrDefaultAsync(item => item.PurchaseId == key);

            _context.Purchases.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> StoresLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Stores
                         orderby i.StoreTitle
                         select new {
                             Value = i.StoreId,
                             Text = i.StoreTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> VendorsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Vendors
                         orderby i.VendorTitle
                         select new {
                             Value = i.VendorId,
                             Text = i.VendorTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Purchase model, IDictionary values) {
            string PURCHASE_ID = nameof(Purchase.PurchaseId);
            string PURCHASE_SERIAL = nameof(Purchase.PurchaseSerial);
            string PURCHASEDATE = nameof(Purchase.Purchasedate);
            string STORE_ID = nameof(Purchase.StoreId);
            string VENDOR_ID = nameof(Purchase.VendorId);
            string TOTAL = nameof(Purchase.Total);
            string DISCOUNT = nameof(Purchase.Discount);
            string NET = nameof(Purchase.Net);
            string REMARKS = nameof(Purchase.Remarks);

            if(values.Contains(PURCHASE_ID)) {
                model.PurchaseId = Convert.ToInt32(values[PURCHASE_ID]);
            }

            if(values.Contains(PURCHASE_SERIAL)) {
                model.PurchaseSerial = Convert.ToString(values[PURCHASE_SERIAL]);
            }

            if(values.Contains(PURCHASEDATE)) {
                model.Purchasedate = values[PURCHASEDATE] != null ? Convert.ToDateTime(values[PURCHASEDATE]) : (DateTime?)null;
            }

            if(values.Contains(STORE_ID)) {
                model.StoreId = values[STORE_ID] != null ? Convert.ToInt32(values[STORE_ID]) : (int?)null;
            }

            if(values.Contains(VENDOR_ID)) {
                model.VendorId = values[VENDOR_ID] != null ? Convert.ToInt32(values[VENDOR_ID]) : (int?)null;
            }

            if(values.Contains(TOTAL)) {
                model.Total = values[TOTAL] != null ? Convert.ToDouble(values[TOTAL], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(DISCOUNT)) {
                model.Discount = values[DISCOUNT] != null ? Convert.ToDouble(values[DISCOUNT], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(NET)) {
                model.Net = values[NET] != null ? Convert.ToDouble(values[NET], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
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