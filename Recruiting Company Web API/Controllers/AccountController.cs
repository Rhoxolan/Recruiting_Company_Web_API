using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Services.AccountServices;

namespace Recruiting_Company_Web_API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IJWTService _JWTService;

		public AccountController(IJWTService jWTService, UserManager<IdentityUser> userManager)
		{
			_JWTService = jWTService;
			_userManager = userManager;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register()
		{
			throw new NotImplementedException();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login()
		{
			throw new NotImplementedException();
		}
	}
}
