using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Entities;
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
