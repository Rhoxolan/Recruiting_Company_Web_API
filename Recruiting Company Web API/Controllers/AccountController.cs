using Microsoft.AspNetCore.Mvc;
//using Recruiting_Company_Web_API.Filters;
using Recruiting_Company_Web_API.Models.AccountModels;
using Recruiting_Company_Web_API.Services.AccountServices.AccountService;
using Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;
		private readonly IJWTService _JWTService;

		public AccountController(IJWTService jWTService, IAccountService accountService)
		{
			_JWTService = jWTService;
			_accountService = accountService;
		}

		[HttpPost("register")]
		//[ValidateModelFilter] //Убрать, если не возникнет необходимости в использовании
		public async Task<IActionResult> Register(RegisterModel model)
		{
			try
			{
				var result = await _accountService.CreateUserAsync(model);
				if (!result.result.Succeeded)
				{
					return BadRequest(new { error = result.result.Errors.First().Description });
				}
				var token = _JWTService.GenerateJWTToken(result.user);
				return Ok(new { token });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("login")]
		//[ValidateModelFilter]
		public async Task<IActionResult> Login(LoginModel model)
		{
			try
			{
				var result = await _accountService.SignInUserAsync(model);
				if (result.user == null || !result.result)
				{
					return Unauthorized("Authentication failed!");
				}
				var token = _JWTService.GenerateJWTToken(result.user);
				return Ok(new { token });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
