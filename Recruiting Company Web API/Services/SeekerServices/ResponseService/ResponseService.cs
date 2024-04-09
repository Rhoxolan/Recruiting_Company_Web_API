using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.ResponseService
{
	public class ResponseService(UserManager<Seeker> userManager, ApplicationContext context) : IResponseService
	{
		public async Task<(bool, bool, bool, dynamic?)> RespondToVacancyAsync(ResponseModel model, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			bool findCVResult = false;
			dynamic? response = null;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var vacancy = await context.Vacancies.FindAsync(model.VacancyId);
				if (findVacancyResult = vacancy != null)
				{
					var cv = await context.CVs
						.Where(c => c.SeekerID == seeker!.Id)
						.Where(c => c.Id == model.CVId)
						.FirstOrDefaultAsync();
					if (findCVResult = cv != null)
					{
						var responseEntity = new Response
						{
							ResponseTime = DateTime.Now,
							Vacancy = vacancy!,
							CV = cv!
						};
						await context.Responses.AddAsync(responseEntity);
						await context.SaveChangesAsync();
						response = new
						{
							responseEntity.Id,
							responseEntity.ResponseTime,
							responseEntity.VacancyID,
							CVId = cv!.Id,
							CVUploadDate = cv.UploadDate,
							File = cv.File != null ? Convert.ToBase64String(cv.File) : null,
							Format = cv.File != null ? cv.FileFormat : null,
							IsFile = cv.File != null,
							IsLink = cv.Link != null,
							Link = cv.Link ?? null
						};
					}
				}
			}
			return (findUserResult, findVacancyResult, findCVResult, response);
		}

		public async Task<(bool, IEnumerable<dynamic>?)> GetResponsesAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? responses = null;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				responses = await context.Responses
					.Where(c => c.CV.SeekerID == seeker!.Id)
					.Select(r => new
					{
						r.Id,
						r.ResponseTime,
						r.VacancyID,
						r.CVID,
						CVUploadDate = r.CV.UploadDate,
						File = r.CV.File != null ? Convert.ToBase64String(r.CV.File) : null,
						Format = r.CV.File != null ? r.CV.FileFormat : null,
						IsFile = r.CV.File != null,
						IsLink = r.CV.Link != null,
						Link = r.CV.Link ?? null
					}).ToListAsync();
			}
			return (findUserResult, responses);
		}

		public async Task<(bool, bool)> CheckIsRespondedAsync(ulong vacancyId, string name)
		{
			bool findUserResult;
			bool isResponded = false;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				isResponded = await context.Responses
					.Where(r => r.CV.SeekerID == seeker!.Id)
					.Where(r => r.VacancyID == vacancyId)
					.AnyAsync();
			}
			return (findUserResult, isResponded);
		}
	}
}
