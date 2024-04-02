using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Models.SeekerModels;
using Recruiting_Company_Web_API.Services.SeekerServices.CVService;
using Recruiting_Company_Web_API.Services.SeekerServices.ResponseService;
using Recruiting_Company_Web_API.Services.SeekerServices.TabsService;
using System.Security.Claims;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SeekerController : ControllerBase
	{
		private readonly ICVService _cvService;
		private readonly IResponseService _responseService;
		private readonly ITabsService _tabsService;

		public SeekerController(ICVService cvService, IResponseService responseService, ITabsService tabsService)
		{
			_cvService = cvService;
			_responseService = responseService;
			_tabsService = tabsService;
		}

		[HttpGet("GetCVs")]
		public async Task<IActionResult> GetCVs()
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, cvs) = await _cvService.GetCVsAsync(userNameClaim!.Value);
				if (!findUserResult || cvs == null)
				{
					return BadRequest();
				}
				return Ok(new { cvs });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("AddCV")]
		public async Task<IActionResult> UploadCV(CVModel model)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (modelValidResult, findUserResult, cv) = await _cvService.UploadCVAsync(model, userNameClaim!.Value);
				if (!modelValidResult || !findUserResult || cv == null)
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

		[HttpDelete("DeleteCV/{id}")]
		public async Task<IActionResult> DeleteCV(ulong id)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, findCVResult) = await _cvService.DeleteCVAsync(id, userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				if (!findCVResult)
				{
					return NotFound();
				}
				return Ok();
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("VacancyResponding")]
		public async Task<IActionResult> RespondToVacancy(ResponseModel model)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, findVacancyResult, findCVResult, response)
					= await _responseService.RespondToVacancyAsync(model, userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				if (!findVacancyResult || !findCVResult)
				{
					return NotFound();
				}
				if (response == null)
				{
					return BadRequest();
				}
				return Ok(new { response });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpGet("GetResponses")]
		public async Task<IActionResult> GetResponses()
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, responses) = await _responseService.GetResponsesAsync(userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				return Ok(new { responses });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpGet("GetTabs")]
		public async Task<IActionResult> GetTabs()
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, tabs) = await _tabsService.GetTabsAsync(userNameClaim!.Value);
				if (!findUserResult || tabs == null)
				{
					return BadRequest();
				}
				return Ok(new { tabs });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("AddTab")]
		public async Task<IActionResult> AddVacansyToTab(TabModel model)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, findVacancyResult) = await _tabsService.AddVacansyToTabAsync(model, userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				if (!findVacancyResult)
				{
					return NotFound();
				}
				return Ok();
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpDelete("DeleteTab/{vacancyId}")]
		public async Task<IActionResult> DeleteTab(ulong vacancyId)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, findVacancyResult, findTabResult)
					= await _tabsService.DeleteTabAsync(vacancyId, userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				if (!findVacancyResult || !findTabResult)
				{
					return NotFound();
				}
				return Ok();
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
