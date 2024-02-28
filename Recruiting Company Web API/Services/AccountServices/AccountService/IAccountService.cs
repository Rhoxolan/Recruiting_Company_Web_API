using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Models.AccountModels;

namespace Recruiting_Company_Web_API.Services.AccountServices.AccountService
{
	public interface IAccountService
	{
		Task<(IdentityResult result, IdentityUser user)> CreateUserAsync(RegisterModel model);

		Task<(bool result, IdentityUser? user)> SignInUserAsync(LoginModel model);
	}
}
