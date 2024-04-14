using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;
using System.Collections.Generic;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public class EmployerService(ApplicationContext context, UserManager<Employer> userManager) : IEmployerService
	{
		public async Task<ServiceResult<IEnumerable<dynamic>>> GetVacanciesAsync(string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<IEnumerable<dynamic>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancies = await GetVacancies(employer.Id)
				.Select(v => new
				{
					v.Id,
					v.CategoryID,
					v.CreateDate,
					v.Title,
					v.Location,
					v.Salary,
					v.PhoneNumber,
					v.EMail,
					v.Description
				}).ToListAsync();
			return ServiceResult<IEnumerable<dynamic>>.Success(vacancies);
		}

		public async Task<ServiceResult<dynamic>> AddVacancyAsync(VacancyModel model, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var category = await context.Categories.FindAsync(model.CategoryID);
			if (category == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound, "Category not found!");
			}
			var vacancyEntity = new Vacancy
			{
				CreateDate = DateTime.Now,
				Category = category,
				Title = model.Title,
				Location = model.Location,
				Salary = model.Salary,
				PhoneNumber = model.PhoneNumber,
				EMail = model.EMail,
				Description = model.Description,
				Employer = employer!
			};
			await context.Vacancies.AddAsync(vacancyEntity);
			await context.SaveChangesAsync();
			var vacancy = new
			{
				vacancyEntity.Id,
				CategoryID = vacancyEntity.Category.Id,
				vacancyEntity.CreateDate,
				vacancyEntity.Title,
				vacancyEntity.Salary,
				vacancyEntity.PhoneNumber,
				vacancyEntity.EMail,
				vacancyEntity.Description
			};
			return ServiceResult<dynamic>.Success(vacancy);
		}

		public async Task<ServiceResult<dynamic>> EditVacansyAsync(VacancyModel model, string name)
		{
			if (model.Id == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.BadModel, "Id is null!");
			}
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var category = await context.Categories.FindAsync(model.CategoryID);
			if (category == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound, "Category not found!");
			}
			var vacancyEntity = await GetVacancies(employer.Id, model.Id).FirstOrDefaultAsync();
			if (vacancyEntity == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			vacancyEntity.Title = model.Title;
			vacancyEntity.Location = model.Location;
			vacancyEntity.Category = category;
			vacancyEntity.CategoryID = category.Id;
			vacancyEntity.Salary = model.Salary;
			vacancyEntity.PhoneNumber = model.PhoneNumber;
			vacancyEntity.EMail = model.EMail;
			vacancyEntity.Description = model.Description;
			await context.SaveChangesAsync();
			var vacancy = new
			{
				vacancyEntity.Id,
				vacancyEntity.CategoryID,
				vacancyEntity.CreateDate,
				vacancyEntity.Title,
				vacancyEntity.Salary,
				vacancyEntity.PhoneNumber,
				vacancyEntity.EMail,
				vacancyEntity.Description
			};
			return ServiceResult<dynamic>.Success(vacancy);
		}

		public async Task<ServiceResult> DeleteVacancyAsync(ulong id, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await GetVacancies(employer.Id, id).FirstOrDefaultAsync();
			if (vacancy == null)
			{
				return ServiceResult.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			context.Vacancies.Remove(vacancy);
			await context.SaveChangesAsync();
			return ServiceResult.Success();
		}

		public async Task<ServiceResult<IEnumerable<dynamic>>> GetVacancyResponsesAsync(ulong id, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<IEnumerable<dynamic>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await GetVacancies(employer.Id, id).FirstOrDefaultAsync();
			if (vacancy == null)
			{
				return ServiceResult<IEnumerable<dynamic>>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var responses = await GetResponses(employer.Id, vacancyId: vacancy.Id)
				.Select(r => new
				{
					r.Id,
					r.ResponseTime,
					IsLink = r.CV.Link != null,
					IsFile = r.CV.File != null,
					r.CV.Link
				}).ToListAsync();
			return ServiceResult<IEnumerable<dynamic>>.Success(responses);
		}

		public async Task<ServiceResult<dynamic>> GetVacancyResponseCVFileAsync(ulong id, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var responseCVFile = await GetResponses(employer.Id, id: id)
				.Select(r => new
				{
					FileName = string.Format("CV_to_Vacansy_{0}",
					r.Vacancy.Title.Length > 7 ? $"{r.Vacancy.Title.Substring(0, 7)}..._" : r.Vacancy.Title),
					r.CV.FileFormat,
					File = r.CV.File != null ? Convert.ToBase64String(r.CV.File) : null
				}).FirstOrDefaultAsync();
			if (responseCVFile == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound, "The response CV file not found!");
			}
			return ServiceResult<dynamic>.Success(responseCVFile);
		}

		private IQueryable<Vacancy> GetVacancies(string userID, ulong? id = null)
		{
			return context.Vacancies
				.Where(v => v.EmployerID == userID)
				.Where(v => id == null || v.Id == id);
		}

		private IQueryable<Response> GetResponses(string userID, ulong? id = null, ulong? vacancyId = null)
		{
			return context.Responses
				.Where(r => r.Vacancy.EmployerID == userID)
				.Where(r => id == null || r.Id == id)
				.Where(r => vacancyId == null || r.Vacancy.Id == vacancyId);
		}
	}
}
