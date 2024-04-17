using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Models.AccountModels;

namespace Recruiting_Company_Web_API.Services.AccountServices.AccountService
{
	public interface IAccountService
	{
		Task<ServiceResult<IdentityUser>> CreateUserAsync(RegisterModel model);

		Task<ServiceResult<IdentityUser>> SignInUserAsync(LoginModel model);
	}
}
