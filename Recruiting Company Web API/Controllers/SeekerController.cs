using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Models.SeekerModels;
using Recruiting_Company_Web_API.Services.SeekerServices.SeekerService;
using System.Security.Claims;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SeekerController : ControllerBase
	{
		private readonly ISeekerService _seekerService;

		public SeekerController(ISeekerService seekerService)
		{
			_seekerService = seekerService;
		}

		[HttpPost("AddCV")]
		public async Task<IActionResult> UploadCV(CVModel model)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (modelValidResult, findUserResult, cv) = await _seekerService.UploadCVAsync(model, userNameClaim!.Value);
				if(!modelValidResult || !findUserResult || cv == null)
				{
					return BadRequest();
				}
				return Ok(new { cv });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
