using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AssetProject.Areas.Admin.Pages.ReportsManagement
{
    public class MergeTwoReportModel : PageModel
    {
       
            public MergeTwoReportModel()
            {
               
            }
        public int CheckInParam { get; set; }
        public int CheckOutParam { get; set; }

        public IActionResult OnGet(int CheckInId, int CheckOutId)
        {
            CheckInParam = CheckInId;
            CheckOutParam = CheckOutId;
            return Page();

        }

    }
    
}
