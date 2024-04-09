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
	public class SeekerController(ICVService cvService, IResponseService responseService,
		ITabsService tabsService) : ControllerBase
	{
		[HttpGet("GetCVs")]
		public async Task<IActionResult> GetCVs()
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, cvs) = await cvService.GetCVsAsync(userNameClaim!.Value);
			if (!findUserResult || cvs == null)
			{
				return BadRequest();
			}
			return Ok(new { cvs });
		}

		[HttpPost("AddCV")]
		public async Task<IActionResult> UploadCV(CVModel model)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (modelValidResult, findUserResult, cv) = await cvService.UploadCVAsync(model, userNameClaim!.Value);
			if (!modelValidResult || !findUserResult || cv == null)
			{
				return BadRequest();
			}
			return Ok(new { cv });
		}

		[HttpDelete("DeleteCV/{id}")]
		public async Task<IActionResult> DeleteCV(ulong id)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, findCVResult) = await cvService.DeleteCVAsync(id, userNameClaim!.Value);
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

		[HttpPost("VacancyResponding")]
		public async Task<IActionResult> RespondToVacancy(ResponseModel model)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, findVacancyResult, findCVResult, response)
				= await responseService.RespondToVacancyAsync(model, userNameClaim!.Value);
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

		[HttpGet("GetResponses")]
		public async Task<IActionResult> GetResponses()
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, responses) = await responseService.GetResponsesAsync(userNameClaim!.Value);
			if (!findUserResult)
			{
				return BadRequest();
			}
			return Ok(new { responses });
		}

		[HttpGet("IsResponsed/{vacancyId}")]
		public async Task<IActionResult> IsResponded(ulong vacancyId)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, isResponded) = await responseService.CheckIsRespondedAsync(vacancyId, userNameClaim!.Value);
			if (!findUserResult)
			{
				return BadRequest();
			}
			return Ok(new { isResponded });
		}

		[HttpGet("GetTabs")]
		public async Task<IActionResult> GetTabs()
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, tabs) = await tabsService.GetTabsAsync(userNameClaim!.Value);
			if (!findUserResult || tabs == null)
			{
				return BadRequest();
			}
			return Ok(new { tabs });
		}

		[HttpPost("AddTab")]
		public async Task<IActionResult> AddVacansyToTab(TabModel model)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, findVacancyResult) = await tabsService.AddVacansyToTabAsync(model, userNameClaim!.Value);
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

		[HttpDelete("DeleteTab/{vacancyId}")]
		public async Task<IActionResult> DeleteTab(ulong vacancyId)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, findVacancyResult, findTabResult)
				= await tabsService.DeleteTabAsync(vacancyId, userNameClaim!.Value);
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

		[HttpGet("IsNoted/{vacancyId}")]
		public async Task<IActionResult> IsNoted(ulong vacancyId)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, isNoted) = await tabsService.CheckIsNotedAsync(vacancyId, userNameClaim!.Value);
			if (!findUserResult)
			{
				return BadRequest();
			}
			return Ok(new { isNoted });
		}
	}
}
