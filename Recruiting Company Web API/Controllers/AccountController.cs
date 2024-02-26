using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.AccountModels;
using Recruiting_Company_Web_API.Services.AccountServices;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<Employer> _employerManager;
		private readonly UserManager<Seeker> _seekerManager;
		private readonly IJWTService _JWTService;

		public AccountController(UserManager<Employer> employerManager, UserManager<Seeker> seekerManager, IJWTService jWTService)
		{
			_employerManager = employerManager;
			_seekerManager = seekerManager;
			_JWTService = jWTService;
		}

		[HttpPost("register")]
		//Фильтр
		public async Task<IActionResult> Register(RegisterModel model)
		{
			try
			{
				IdentityUser? user = null;
				IdentityResult? result = null;
				if (model.AccountType == 1)
				{
					user = new Seeker
					{
						UserName = model.Login,
						Age = model.Age!.Value,
						Name = model.Name!
					};
					result = await _seekerManager.CreateAsync((Seeker)user, model.Password);
				}
				else if (model.AccountType == 2)
				{
					user = new Employer
					{
						UserName = model.Login,
						CompanyName = model.CompanyName!
					};
					result = await _employerManager.CreateAsync((Employer)user, model.Password);
				}
				if (result == null || user == null || !result.Succeeded)
				{
					return BadRequest(new { result?.Errors });
				}
				var token = _JWTService.GenerateJWTToken(user);
				return Ok(new { token });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("login")]
		//Фильтр
		public async Task<IActionResult> Login(LoginModel model)
		{
			try
			{
				IdentityUser? user = null;
				bool checkPassword = false;
				if (model.AccountType == 1)
				{
					user = await _seekerManager.FindByNameAsync(model.Login);
					if (user != null)
					{
						checkPassword = await _seekerManager.CheckPasswordAsync((Seeker)user, model.Password);
					}
				}
				else if (model.AccountType == 2)
				{
					user = await _employerManager.FindByNameAsync(model.Login);
					if (user != null)
					{
						checkPassword = await _employerManager.CheckPasswordAsync((Employer)user, model.Password);
					}
				}
				if (user == null || !checkPassword)
				{
					return Unauthorized();
				}
				var token = _JWTService.GenerateJWTToken(user);
				return Ok(new { token });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
