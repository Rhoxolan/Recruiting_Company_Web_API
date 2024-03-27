﻿using Microsoft.AspNetCore.Authorization;
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

		[HttpGet("GetCVs")]
		public async Task<IActionResult> GetCVs()
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, cvs) = await _seekerService.GetCVsAsync(userNameClaim!.Value);
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

		[HttpPost("AddTab")]
		public async Task<IActionResult> AddVacansyToTab(ulong id)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, findVacancyResult) = await _seekerService.AddVacansyToTabAsync(id, userNameClaim!.Value);
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
	}
}
