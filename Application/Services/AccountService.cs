using Microsoft.AspNetCore.Identity;

namespace PlaneStore.Application.Services
{
	public interface IAccountService
	{
		Task<bool> SignInAsync(string userName, string password);
		Task SignOutAsync();
	}

	internal class AccountService : IAccountService
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<bool> SignInAsync(string userName, string password)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is not null)
			{
				await SignOutAsync();

				var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
				if (signInResult.Succeeded)
				{
					return true;
				}
			}

			return false;
		}

		public async Task SignOutAsync() => await _signInManager.SignOutAsync();
	}
}
