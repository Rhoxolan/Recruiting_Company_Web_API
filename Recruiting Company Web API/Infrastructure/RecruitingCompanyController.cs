﻿using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Services;
using System.Security.Claims;
using static Recruiting_Company_Web_API.Services.ServiceErrorType;

namespace Recruiting_Company_Web_API.Infrastructure
{
	public class RecruitingCompanyController : ControllerBase
	{
		protected string UserName
		{
			get => User.FindFirst(ClaimTypes.Name)!.Value;
		}

		protected IActionResult ProcessResult(ServiceResultBase result, Func<IActionResult> ok)
		{
			if (result.Succeded && result.Error.ErrorType == None)
			{
				return ok();
			}
			else if (result.Error.ErrorType == EntityNotFound)
			{
				return string.IsNullOrEmpty(result.Error.Message) ? NotFound() : NotFound(new { result.Error.Message });
			}
			else if (result.Error.ErrorType is UserNotFound or BadModel or Unknown)
			{
				return string.IsNullOrEmpty(result.Error.Message) ? BadRequest() : BadRequest(new { result.Error.Message });
			}
			else
			{
				throw new Exception();
			}
		}
	}
}