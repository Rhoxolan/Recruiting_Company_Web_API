using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Models.AccountModels;

namespace Recruiting_Company_Web_API.Services.AccountServices.AccountService
{
	public interface IAccountService
	{
		Task<IdentityUser> CreateUserAsync(RegisterModel model);

		Task<IdentityUser> SignInUserAsync(LoginModel model);
	}
}
