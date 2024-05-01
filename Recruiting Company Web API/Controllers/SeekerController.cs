using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Infrastructure;
using Recruiting_Company_Web_API.Models.SeekerModels;
using Recruiting_Company_Web_API.Services.SeekerServices.CVService;
using Recruiting_Company_Web_API.Services.SeekerServices.ResponseService;
using Recruiting_Company_Web_API.Services.SeekerServices.TabsService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SeekerController(ICVService cvService, IResponseService responseService,
		ITabsService tabsService) : RecruitingCompanyController
	{
		[HttpGet("GetCVs")]
		public async Task<IActionResult> GetCVs()
		{
			var result = await cvService.GetCVsAsync(UserName);
			return ProcessResult(result, () => Ok(new { cvs = result.Value }));
		}

		[HttpGet("GetCVsFile/{id}")]
		public async Task<IActionResult> GetCVsFile(ulong id)
		{
			var result = await cvService.GetCVsFileAsync(id, UserName);
			return ProcessResult(result, () => Ok(new { cvsFile = result.Value }));
		}

		[HttpPost("AddCV")]
		public async Task<IActionResult> UploadCV(CVModel model)
		{
			var result = await cvService.UploadCVAsync(model, UserName);
			return ProcessResult(result, () => Ok(new { cv = result.Value }));
		}

		[HttpDelete("DeleteCV/{id}")]
		public async Task<IActionResult> DeleteCV(ulong id)
		{
			var result = await cvService.DeleteCVAsync(id, UserName);
			return ProcessResult(result, Ok);
		}

		[HttpPost("VacancyResponding")]
		public async Task<IActionResult> RespondToVacancy(ResponseModel model)
		{
			var result = await responseService.RespondToVacancyAsync(model, UserName);
			return ProcessResult(result, () => Ok(new { response = result.Value }));
		}

		[HttpGet("GetResponses")]
		public async Task<IActionResult> GetResponses()
		{
			var result = await responseService.GetResponsesAsync(UserName);
			return ProcessResult(result, () => Ok(new { responses = result.Value }));
		}

		[HttpGet("GetResponseCVFile/{id}")]
		public async Task<IActionResult> GetResponseCVFile(ulong id)
		{
			var result = await responseService.GetResponseCVFileAsync(id, UserName);
			return ProcessResult(result, () => Ok(new { responseCVFile = result.Value }));
		}

		[HttpGet("IsResponsed/{vacancyId}")]
		public async Task<IActionResult> IsResponded(ulong vacancyId)
		{
			var result = await responseService.CheckIsRespondedAsync(vacancyId, UserName);
			return ProcessResult(result, () => Ok(new { isResponded = result.Value }));
		}

		[HttpGet("GetTabs")]
		public async Task<IActionResult> GetTabs()
		{
			var result = await tabsService.GetTabsAsync(UserName);
			return ProcessResult(result, () => Ok(new { tabs = result.Value }));
		}

		[HttpPost("AddTab")]
		public async Task<IActionResult> AddVacansyToTab(TabModel model)
		{
			var result = await tabsService.AddVacansyToTabAsync(model, UserName);
			return ProcessResult(result, Ok);
		}

		[HttpDelete("DeleteTab/{vacancyId}")]
		public async Task<IActionResult> DeleteTab(ulong vacancyId)
		{
			var result = await tabsService.DeleteTabAsync(vacancyId, UserName);
			return ProcessResult(result, Ok);
		}

		[HttpGet("IsNoted/{vacancyId}")]
		public async Task<IActionResult> IsNoted(ulong vacancyId)
		{
			var result = await tabsService.CheckIsNotedAsync(vacancyId, UserName);
			return ProcessResult(result, () => Ok(new { isNoted = result.Value }));
		}
	}
}
