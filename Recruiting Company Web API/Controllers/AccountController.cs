using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Filters;
using Recruiting_Company_Web_API.Models.AccountModels;
using Recruiting_Company_Web_API.Services.AccountServices.AccountService;
using Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService;
using Recruiting_Company_Web_API.Types.Exceptions;

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
		[ValidateModelFilter]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			try
			{
				var user = await _accountService.CreateUserAsync(model);
				var token = _JWTService.GenerateJWTToken(user);
				return Ok(new { token });
			}
			catch (RecruitingCompanyAuthenticationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("login")]
		[ValidateModelFilter]
		public async Task<IActionResult> Login(LoginModel model)
		{
			try
			{
				var user = await _accountService.SignInUserAsync(model);
				var token = _JWTService.GenerateJWTToken(user);
				return Ok(new { token });
			}
			catch (RecruitingCompanyAuthenticationException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
