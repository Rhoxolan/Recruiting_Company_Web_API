using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Infrastructure;
using Recruiting_Company_Web_API.Models.AccountModels;
using Recruiting_Company_Web_API.Services.AccountServices.AccountService;
using Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(IJWTService jWTService, IAccountService accountService) : RecruitingCompanyController
	{
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			var result = await accountService.CreateUserAsync(model);
			return ProcessResult(result, () => Ok(new {
				token = jWTService.GenerateJWTToken(result.Value!)
			}));
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel model)
		{
			var result = await accountService.SignInUserAsync(model);
			return ProcessResult(result, () => Ok(new {
				token = jWTService.GenerateJWTToken(result.Value!) 
			}));
		}
	}
}
