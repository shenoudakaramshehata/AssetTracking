using AssetProject.Data;
using AssetProject.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Areas.Admin.Pages.PurchaseManagement
{
    [Authorize]
    public class NewPurchaseModel : PageModel
    {
        private readonly AssetContext _context;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public Purchase purchase { get; set; }
        public static int count = 0;
        public static List<PurchaseAsset> PurchaseAssetsList = new List<PurchaseAsset>();

        public NewPurchaseModel(AssetContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            foreach (var item in PurchaseAssetsList)
            {
                item.PurchaseAssetId = 0;
            }
            if (!ModelState.IsValid)
                return Page();
            try
            {
                purchase.PurchaseAssets = PurchaseAssetsList;
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
                //Save Detials
                _toastNotification.AddSuccessToastMessage("Purchase Created Successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return RedirectToPage("PurchaseList");
        }

       
        
        public IActionResult OnPostGridInsert(string values)
        {
            var model = new PurchaseAsset();
            model.PurchaseAssetId = count;
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            
 
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));
            PurchaseAssetsList.Add(model);
            count++;
            return new OkResult();
        }
        
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions)
        {
           
            return new JsonResult(DataSourceLoader.Load(PurchaseAssetsList, loadOptions));
        }
        public IActionResult OnPutGridUpdate(int key, string values)
        {
            var model = PurchaseAssetsList.Find(k => k.PurchaseAssetId == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            PurchaseAssetsList.RemoveAt(key);
            PurchaseAssetsList.Add(model);
            return new OkResult();
        }

        public IActionResult OnDeleteGridDelete(int key)
        {
            
            PurchaseAssetsList.RemoveAt(key);
            count--;
            return new OkResult();
        }


        private void PopulateModel(PurchaseAsset model, IDictionary values)
        {
            string PURCHASE_ASSET_ID = nameof(PurchaseAsset.PurchaseAssetId);
            string PURCHASE_ID = nameof(PurchaseAsset.PurchaseId);
            string ITEM_ID = nameof(PurchaseAsset.ItemId);
            string QUANTITY = nameof(PurchaseAsset.Quantity);
            string PRICE = nameof(PurchaseAsset.Price);
            string TOTAL = nameof(PurchaseAsset.Total);
            string DISCOUNT = nameof(PurchaseAsset.Discount);
            string NET = nameof(PurchaseAsset.Net);
            string REMARKS = nameof(PurchaseAsset.Remarks);

            if (values.Contains(PURCHASE_ASSET_ID))
            {
                model.PurchaseAssetId = Convert.ToInt32(values[PURCHASE_ASSET_ID]);
            }

            if (values.Contains(PURCHASE_ID))
            {
                model.PurchaseId = Convert.ToInt32(values[PURCHASE_ID]);
            }

            if (values.Contains(ITEM_ID))
            {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if (values.Contains(QUANTITY))
            {
                model.Quantity = values[QUANTITY] != null ? Convert.ToDouble(values[QUANTITY], CultureInfo.InvariantCulture) : (double?)null;
            }

            if (values.Contains(PRICE))
            {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if (values.Contains(TOTAL))
            {
                model.Total = values[TOTAL] != null ? Convert.ToDouble(values[TOTAL], CultureInfo.InvariantCulture) : (double?)null;
            }

            if (values.Contains(DISCOUNT))
            {
                model.Discount = values[DISCOUNT] != null ? Convert.ToDouble(values[DISCOUNT], CultureInfo.InvariantCulture) : (double?)null;
            }

            if (values.Contains(NET))
            {
                model.Net = values[NET] != null ? Convert.ToDouble(values[NET], CultureInfo.InvariantCulture) : (double?)null;
            }

            if (values.Contains(REMARKS))
            {
                model.Remarks = Convert.ToString(values[REMARKS]);
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
        //public JsonResult Test(FormCollection data)
        //{
        //    System.Diagnostics.Debug.WriteLine(data["text"]);

        //    var gridData = JsonConvert.DeserializeObject<List<PurchaseAsset>>(data["gridData"]);

        //    return Json(gridData[0].Price);
        //}
    }
}
