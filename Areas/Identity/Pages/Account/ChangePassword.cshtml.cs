using AssetProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using AssetProject.Models;
using NToastNotify;

namespace AssetProject.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        [BindProperty]
        public ChangePasswordVM changePasswordVM { get; set; }
        public ChangePasswordModel(UserManager<ApplicationUser> userManager,IToastNotification toastNotification, SignInManager<ApplicationUser> signInManager )
        {
            _userManger = userManager;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
        }
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var user = await _userManger.GetUserAsync(User);
            var result = await _userManger.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            await _signInManager.RefreshSignInAsync(user);
            _toastNotification.AddSuccessToastMessage("Password Updated Successfully");
             return RedirectToPage("/index");

        }
    }
}
