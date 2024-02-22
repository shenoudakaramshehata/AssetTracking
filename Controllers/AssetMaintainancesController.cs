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
    public class AssetMaintainancesController : Controller
    {
        private AssetContext _context;

        public AssetMaintainancesController(AssetContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? AssetId, DataSourceLoadOptions loadOptions) {
            if(AssetId != null)
            {
                var assetmaintainances = _context.AssetMaintainances.Where(a => a.AssetId == AssetId).Select(i => new {
                    i.AssetMaintainanceId,
                    i.AssetMaintainanceTitle,
                    i.AssetMaintainanceDetails,
                    i.AssetMaintainanceDueDate,
                    i.MaintainanceStatusId,
                    i.AssetMaintainanceDateCompleted,
                    i.AssetMaintainanceRepairesCost,
                    i.AssetMaintainanceRepeating,
                    i.AssetMaintainanceFrequencyId,
                    i.TechnicianId,
                    i.AssetId,
                    i.WeeklyPeriod,
                    i.WeekDayId,
                    i.MonthlyPeriod,
                    i.MonthlyDay,
                    i.MonthId,
                    i.YearlyDay,
                    i.ScheduleDate
                });
                return Json(await DataSourceLoader.LoadAsync(assetmaintainances, loadOptions));
            }
            else
            {
                var assetmaintainances = _context.AssetMaintainances.Select(i => new {
                    i.AssetMaintainanceId,
                    i.AssetMaintainanceTitle,
                    i.AssetMaintainanceDetails,
                    i.AssetMaintainanceDueDate,
                    i.MaintainanceStatusId,
                    i.AssetMaintainanceDateCompleted,
                    i.AssetMaintainanceRepairesCost,
                    i.AssetMaintainanceRepeating,
                    i.AssetMaintainanceFrequencyId,
                    i.TechnicianId,
                    i.AssetId,
                    i.WeeklyPeriod,
                    i.WeekDayId,
                    i.MonthlyPeriod,
                    i.MonthlyDay,
                    i.MonthId,
                    i.YearlyDay
                });
                return Json(await DataSourceLoader.LoadAsync(assetmaintainances, loadOptions));
            }
           

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "AssetMaintainanceId" };
            // loadOptions.PaginateViaPrimaryKey = true;

          
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AssetMaintainance();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AssetMaintainances.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.AssetMaintainanceId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AssetMaintainances.FirstOrDefaultAsync(item => item.AssetMaintainanceId == key);
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
            var model = await _context.AssetMaintainances.FirstOrDefaultAsync(item => item.AssetMaintainanceId == key);

            _context.AssetMaintainances.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> MaintainanceStatusesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.MaintainanceStatuses
                         orderby i.MaintainanceStatusTitle
                         select new {
                             Value = i.MaintainanceStatusId,
                             Text = i.MaintainanceStatusTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> AssetMaintainanceFrequenciesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.AssetMaintainanceFrequencies
                         orderby i.AssetMaintainanceFrequencyTitle
                         select new {
                             Value = i.AssetMaintainanceFrequencyId,
                             Text = i.AssetMaintainanceFrequencyTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> TechniciansLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Technicians
                         orderby i.FullName
                         select new {
                             Value = i.TechnicianId,
                             Text = i.FullName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> AssetsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Assets
                         orderby i.AssetDescription
                         select new {
                             Value = i.AssetId,
                             Text = i.AssetDescription
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> WeekDaysLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.WeekDays
                         orderby i.WeekDayTitle
                         select new {
                             Value = i.WeekDayId,
                             Text = i.WeekDayTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> MonthsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Months
                         orderby i.MonthTitle
                         select new {
                             Value = i.MonthId,
                             Text = i.MonthTitle
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(AssetMaintainance model, IDictionary values) {
            string ASSET_MAINTAINANCE_ID = nameof(AssetMaintainance.AssetMaintainanceId);
            string ASSET_MAINTAINANCE_TITLE = nameof(AssetMaintainance.AssetMaintainanceTitle);
            string ASSET_MAINTAINANCE_DETAILS = nameof(AssetMaintainance.AssetMaintainanceDetails);
            string ASSET_MAINTAINANCE_DUE_DATE = nameof(AssetMaintainance.AssetMaintainanceDueDate);
            string MAINTAINANCE_STATUS_ID = nameof(AssetMaintainance.MaintainanceStatusId);
            string ASSET_MAINTAINANCE_DATE_COMPLETED = nameof(AssetMaintainance.AssetMaintainanceDateCompleted);
            string ASSET_MAINTAINANCE_REPAIRES_COST = nameof(AssetMaintainance.AssetMaintainanceRepairesCost);
            string ASSET_MAINTAINANCE_REPEATING = nameof(AssetMaintainance.AssetMaintainanceRepeating);
            string ASSET_MAINTAINANCE_FREQUENCY_ID = nameof(AssetMaintainance.AssetMaintainanceFrequencyId);
            string TECHNICIAN_ID = nameof(AssetMaintainance.TechnicianId);
            string ASSET_ID = nameof(AssetMaintainance.AssetId);
            string WEEKLY_PERIOD = nameof(AssetMaintainance.WeeklyPeriod);
            string WEEK_DAY_ID = nameof(AssetMaintainance.WeekDayId);
            string MONTHLY_PERIOD = nameof(AssetMaintainance.MonthlyPeriod);
            string MONTHLY_DAY = nameof(AssetMaintainance.MonthlyDay);
            string MONTH_ID = nameof(AssetMaintainance.MonthId);
            string YEARLY_DAY = nameof(AssetMaintainance.YearlyDay);

            if(values.Contains(ASSET_MAINTAINANCE_ID)) {
                model.AssetMaintainanceId = Convert.ToInt32(values[ASSET_MAINTAINANCE_ID]);
            }

            if(values.Contains(ASSET_MAINTAINANCE_TITLE)) {
                model.AssetMaintainanceTitle = Convert.ToString(values[ASSET_MAINTAINANCE_TITLE]);
            }

            if(values.Contains(ASSET_MAINTAINANCE_DETAILS)) {
                model.AssetMaintainanceDetails = Convert.ToString(values[ASSET_MAINTAINANCE_DETAILS]);
            }

            if(values.Contains(ASSET_MAINTAINANCE_DUE_DATE)) {
                model.AssetMaintainanceDueDate = Convert.ToDateTime(values[ASSET_MAINTAINANCE_DUE_DATE]);
            }

            if(values.Contains(MAINTAINANCE_STATUS_ID)) {
                model.MaintainanceStatusId = Convert.ToInt32(values[MAINTAINANCE_STATUS_ID]);
            }

            if(values.Contains(ASSET_MAINTAINANCE_DATE_COMPLETED)) {
                model.AssetMaintainanceDateCompleted = Convert.ToDateTime(values[ASSET_MAINTAINANCE_DATE_COMPLETED]);
            }

            if(values.Contains(ASSET_MAINTAINANCE_REPAIRES_COST)) {
                model.AssetMaintainanceRepairesCost = Convert.ToDouble(values[ASSET_MAINTAINANCE_REPAIRES_COST], CultureInfo.InvariantCulture);
            }

            if(values.Contains(ASSET_MAINTAINANCE_REPEATING)) {
                model.AssetMaintainanceRepeating = Convert.ToBoolean(values[ASSET_MAINTAINANCE_REPEATING]);
            }

            if(values.Contains(ASSET_MAINTAINANCE_FREQUENCY_ID)) {
                model.AssetMaintainanceFrequencyId = values[ASSET_MAINTAINANCE_FREQUENCY_ID] != null ? Convert.ToInt32(values[ASSET_MAINTAINANCE_FREQUENCY_ID]) : (int?)null;
            }

            if(values.Contains(TECHNICIAN_ID)) {
                model.TechnicianId = Convert.ToInt32(values[TECHNICIAN_ID]);
            }

            if(values.Contains(ASSET_ID)) {
                model.AssetId = Convert.ToInt32(values[ASSET_ID]);
            }

            if(values.Contains(WEEKLY_PERIOD)) {
                model.WeeklyPeriod = values[WEEKLY_PERIOD] != null ? Convert.ToInt32(values[WEEKLY_PERIOD]) : (int?)null;
            }

            if(values.Contains(WEEK_DAY_ID)) {
                model.WeekDayId = values[WEEK_DAY_ID] != null ? Convert.ToInt32(values[WEEK_DAY_ID]) : (int?)null;
            }

            if(values.Contains(MONTHLY_PERIOD)) {
                model.MonthlyPeriod = values[MONTHLY_PERIOD] != null ? Convert.ToInt32(values[MONTHLY_PERIOD]) : (int?)null;
            }

            if(values.Contains(MONTHLY_DAY)) {
                model.MonthlyDay = values[MONTHLY_DAY] != null ? Convert.ToInt32(values[MONTHLY_DAY]) : (int?)null;
            }

            if(values.Contains(MONTH_ID)) {
                model.MonthId = values[MONTH_ID] != null ? Convert.ToInt32(values[MONTH_ID]) : (int?)null;
            }

            if(values.Contains(YEARLY_DAY)) {
                model.YearlyDay = values[YEARLY_DAY] != null ? Convert.ToInt32(values[YEARLY_DAY]) : (int?)null;
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