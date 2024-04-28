using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.DTOs.FileDTOs;
using Recruiting_Company_Web_API.DTOs.ResponseDTOs;
using Recruiting_Company_Web_API.DTOs.VacancyDTOs;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public class EmployerService(ApplicationContext context, UserManager<Employer> userManager) : IEmployerService
	{
		public async Task<ServiceResult<List<VacancyDTO>>> GetVacanciesAsync(string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<List<VacancyDTO>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancies = await GetVacancies(employer.Id)
				.Select(v => new VacancyDTO
				{
					Id = v.Id,
					CategoryID = v.CategoryID,
					CreateDate = v.CreateDate,
					Title = v.Title,
					Location = v.Location,
					Salary = v.Salary,
					PhoneNumber = v.PhoneNumber,
					EMail = v.EMail,
					Description = v.Description
				}).ToListAsync();
			return ServiceResult<List<VacancyDTO>>.Success(vacancies);
		}

		public async Task<ServiceResult<VacancyDTO>> AddVacancyAsync(VacancyModel model, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<VacancyDTO>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var category = await context.Categories.FindAsync(model.CategoryID);
			if (category == null)
			{
				return ServiceResult<VacancyDTO>.Failure(ServiceErrorType.EntityNotFound, "Category not found!");
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
			var vacancy = new VacancyDTO
			{
				Id = vacancyEntity.Id,
				CategoryID = vacancyEntity.Category.Id,
				CreateDate = vacancyEntity.CreateDate,
				Title = vacancyEntity.Title,
				Location = vacancyEntity.Location,
				Salary = vacancyEntity.Salary,
				PhoneNumber = vacancyEntity.PhoneNumber,
				EMail = vacancyEntity.EMail,
				Description = vacancyEntity.Description
			};
			return ServiceResult<VacancyDTO>.Success(vacancy);
		}

		public async Task<ServiceResult<VacancyDTO>> EditVacansyAsync(VacancyModel model, string name)
		{
			if (model.Id == null)
			{
				return ServiceResult<VacancyDTO>.Failure(ServiceErrorType.BadModel, "Id is null!");
			}
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<VacancyDTO>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var category = await context.Categories.FindAsync(model.CategoryID);
			if (category == null)
			{
				return ServiceResult<VacancyDTO>.Failure(ServiceErrorType.EntityNotFound, "Category not found!");
			}
			var vacancyEntity = await GetVacancies(employer.Id, model.Id).FirstOrDefaultAsync();
			if (vacancyEntity == null)
			{
				return ServiceResult<VacancyDTO>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
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
			var vacancy = new VacancyDTO
			{
				Id = vacancyEntity.Id,
				CategoryID = vacancyEntity.CategoryID,
				CreateDate = vacancyEntity.CreateDate,
				Title = vacancyEntity.Title,
				Location = vacancyEntity.Location,
				Salary = vacancyEntity.Salary,
				PhoneNumber = vacancyEntity.PhoneNumber,
				EMail = vacancyEntity.EMail,
				Description = vacancyEntity.Description
			};
			return ServiceResult<VacancyDTO>.Success(vacancy);
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

		public async Task<ServiceResult<List<ResponseDTO>>> GetVacancyResponsesAsync(ulong id, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<List<ResponseDTO>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await GetVacancies(employer.Id, id).FirstOrDefaultAsync();
			if (vacancy == null)
			{
				return ServiceResult<List<ResponseDTO>>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var responses = await GetResponses(employer.Id, vacancyId: vacancy.Id)
				.Select(r => new ResponseDTO
				{
					Id = r.Id,
					ResponseTime = r.ResponseTime,
					IsLink = r.CV.Link != null,
					IsFile = r.CV.File != null,
					Link = r.CV.Link
				}).ToListAsync();
			return ServiceResult<List<ResponseDTO>>.Success(responses);
		}

		public async Task<ServiceResult<FileDTO>> GetVacancyResponseCVFileAsync(ulong id, string name)
		{
			var employer = await userManager.FindByNameAsync(name);
			if (employer == null)
			{
				return ServiceResult<FileDTO>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var responseCVFile = await GetResponses(employer.Id, id: id)
				.Select(r => new FileDTO
				{
					FileName = string.Format("CV_to_Vacansy_{0}", r.Vacancy.Title.Length > 7 ? $"{r.Vacancy.Title.Substring(0, 7)}..._" : r.Vacancy.Title),
					FileFormat = r.CV.FileFormat,
					File = r.CV.File != null ? Convert.ToBase64String(r.CV.File) : null
				}).FirstOrDefaultAsync();
			if (responseCVFile == null)
			{
				return ServiceResult<FileDTO>.Failure(ServiceErrorType.EntityNotFound, "The response CV file not found!");
			}
			return ServiceResult<FileDTO>.Success(responseCVFile);
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
