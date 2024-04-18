using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.ResponseService
{
	public class ResponseService(UserManager<Seeker> userManager, ApplicationContext context) : IResponseService
	{
		public async Task<ServiceResult<dynamic>> RespondToVacancyAsync(ResponseModel model, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await context.Vacancies.FindAsync(model.VacancyId);
			if (vacancy == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var cv = await context.CVs
				.Where(c => c.SeekerID == seeker.Id)
				.Where(c => c.Id == model.CVId)
				.FirstOrDefaultAsync();
			if (cv == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound, "CV not found!");
			}
			var responseEntity = new Response
			{
				ResponseTime = DateTime.Now,
				Vacancy = vacancy!,
				CV = cv!
			};
			await context.Responses.AddAsync(responseEntity);
			await context.SaveChangesAsync();
			var response = new
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
			return ServiceResult<dynamic>.Success(response);
		}

		public async Task<ServiceResult<IEnumerable<dynamic>>> GetResponsesAsync(string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<IEnumerable<dynamic>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var responses = await context.Responses
				.Where(c => c.CV.SeekerID == seeker.Id)
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
			return ServiceResult<IEnumerable<dynamic>>.Success(responses);
		}

		public async Task<ServiceResult<bool>> CheckIsRespondedAsync(ulong vacancyId, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<bool>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await context.Vacancies.FindAsync(vacancyId);
			if (vacancy == null)
			{
				return ServiceResult<bool>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var isResponded = await context.Responses
				.Where(r => r.CV.SeekerID == seeker.Id)
				.Where(r => r.VacancyID == vacancy.Id)
				.AnyAsync();
			return ServiceResult<bool>.Success(isResponded);
		}
	}
}
