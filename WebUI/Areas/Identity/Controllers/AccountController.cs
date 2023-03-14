using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaneStore.WebUI.Areas.Identity.Models;

namespace PlaneStore.WebUI.Areas.Identity.Controllers
{
    public class AccountController : IdentityControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ViewResult Login(string returnUrl)
            => View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Name!);
                if (user is not null)
                {
                    await _signInManager.SignOutAsync();
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password!, false, false);
                    if (signInResult.Succeeded)
                    {
                        return Redirect(model.ReturnUrl ?? "/Admin");
                    }

                    ModelState.AddModelError("", "Invalid user name or password.");
                }
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
