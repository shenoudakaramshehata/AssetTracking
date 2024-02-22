using AssetProject.Data;
using AssetProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssetProject.Areas.Identity.Pages.UserManagment
{
    [Authorize]
    public class UserListModel : PageModel
    {
        ApplicationDbContext _context { set; get; }
        public List<ApplicationUser> Users {set;get;}
        UserManager<ApplicationUser> UserManger;
        public UserListModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await UserManger.FindByIdAsync(userid);
            Users =_context.Users.Where(u => u.TenantId == user.TenantId &&u.Id!=userid).ToList();
            return Page();
        }
    }
}
