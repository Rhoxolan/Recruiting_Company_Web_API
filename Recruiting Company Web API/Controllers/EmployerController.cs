﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Models.EmployerModels;
using Recruiting_Company_Web_API.Services.EmployerServices.EmployerService;
using System.Security.Claims;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EmployerController(IEmployerService employerService) : ControllerBase
	{
		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies()
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, vacancies) = await employerService.GetVacanciesAsync(userNameClaim!.Value);
			if (!findUserResult || vacancies == null)
			{
				return BadRequest();
			}
			return Ok(new { vacancies });
		}

		[HttpPost("AddVacancy")]
		public async Task<IActionResult> AddVacancy(VacancyModel model)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, vacancy) = await employerService.AddVacancyAsync(model, userNameClaim!.Value);
			if (!findUserResult || vacancy == null)
			{
				return BadRequest();
			}
			return Ok(new { vacancy });
		}

		[HttpPatch("EditVacancy")]
		public async Task<IActionResult> EditVacancy(VacancyModel model)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, vacancy) = await employerService.EditVacansyAsync(model, userNameClaim!.Value);
			if (!findUserResult)
			{
				return BadRequest();
			}
			if (vacancy == null)
			{
				return NotFound();
			}
			return Ok(new { vacancy });
		}

		[HttpDelete("DeleteVacancy/{id}")]
		public async Task<IActionResult> DeleteVacancy(ulong id)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, findVacancyResult) = await employerService.DeleteVacancyAsync(id, userNameClaim!.Value);
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

		[HttpGet("Responses/{id}")]
		public async Task<IActionResult> GetResponses(ulong id)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, findVacancyResult, responses) = await employerService
				.GetVacancyResponsesAsync(id, userNameClaim!.Value);
			if (!findUserResult)
			{
				return BadRequest();
			}
			if (!findVacancyResult)
			{
				return NotFound();
			}
			return Ok(new { responses });
		}

		[HttpGet("ResponseCVFile/{id}")]
		public async Task<IActionResult> GetResponseCVFile(ulong id)
		{
			var userNameClaim = User.FindFirst(ClaimTypes.Name);
			var (findUserResult, responseCVFile) = await employerService.GetVacancyResponseCVFileAsync(id, userNameClaim!.Value);
			if (!findUserResult)
			{
				return BadRequest();
			}
			if (responseCVFile == null)
			{
				return NotFound();
			}
			return Ok(new { responseCVFile });
		}
	}
}
