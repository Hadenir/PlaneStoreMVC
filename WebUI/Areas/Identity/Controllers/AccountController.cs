using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Services;
using PlaneStore.WebUI.Areas.Identity.Models;

namespace PlaneStore.WebUI.Areas.Identity.Controllers
{
	public class AccountController : IdentityControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		public ViewResult Login(string returnUrl = "/")
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
				if (await _accountService.SignInAsync(model.UserName!, model.Password!))
				{
					return Redirect(model.ReturnUrl ?? "/Admin");
				}

				ModelState.AddModelError("", "Failed to sign in. Check user name or password.");
			}

			return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _accountService.SignOutAsync();
			return Redirect(returnUrl);
		}
	}
}
