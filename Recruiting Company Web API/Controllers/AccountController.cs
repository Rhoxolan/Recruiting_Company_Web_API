using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Models.AccountModels;
using Recruiting_Company_Web_API.Services.AccountServices.AccountService;
using Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(IJWTService jWTService, IAccountService accountService) : ControllerBase
	{
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			var result = await accountService.CreateUserAsync(model);
			if (!result.result.Succeeded)
			{
				return BadRequest(new { error = result.result.Errors.First().Description });
			}
			var token = jWTService.GenerateJWTToken(result.user);
			return Ok(new { token });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel model)
		{
			var result = await accountService.SignInUserAsync(model);
			if (result.user == null || !result.result)
			{
				return Unauthorized("Authentication failed!");
			}
			var token = jWTService.GenerateJWTToken(result.user);
			return Ok(new { token });
		}
	}
}
