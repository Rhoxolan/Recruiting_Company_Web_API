using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Services.EmployerServices.EmployerService;
using System.Security.Claims;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EmployerController : ControllerBase
	{
		private readonly IEmployerService _employerService;
		private readonly UserManager<Employer> _userManager;

		public EmployerController(IEmployerService employerService, UserManager<Employer> userManager)
		{
			_employerService = employerService;
			_userManager = userManager;
		}

		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies()
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				if (userNameClaim == null)
				{
					return BadRequest();
				}
				var user = await _userManager.FindByNameAsync(userNameClaim.Value);
				if (user == null)
				{
					return BadRequest();
				}
				throw new NotImplementedException();
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
