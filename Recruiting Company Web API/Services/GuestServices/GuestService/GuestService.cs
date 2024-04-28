using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.DTOs.VacancyDTOs;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public class GuestService(ApplicationContext context) : IGuestService
	{
		public async Task<ServiceResult<CommonVacancyDTO>> GetVacancyAsync(ulong id)
		{
			var vacancy = await context.Vacancies
				.Where(v => v.Id == id)
				.Select(v => new CommonVacancyDTO
				{
					Id = v.Id,
					CategoryID = v.CategoryID,
					EmployerID = v.Employer.PublicId,
					Employer = v.Employer.CompanyName,
					CreateDate = v.CreateDate,
					Title = v.Title,
					Location = v.Location,
					Salary = v.Salary,
					PhoneNumber = v.PhoneNumber,
					EMail = v.EMail,
					Description = v.Description
				}).FirstOrDefaultAsync();
			if (vacancy == null)
			{
				return ServiceResult<CommonVacancyDTO>.Failure(ServiceErrorType.EntityNotFound);
			}
			return ServiceResult<CommonVacancyDTO>.Success(vacancy);
		}

		public async Task<ServiceResult<int>> GetVacanciesCountAsync(VacancyRequestParameters requestParameters)
		{
			var vacanciesCount = await GetVacancies(requestParameters).CountAsync();
			return ServiceResult<int>.Success(vacanciesCount);
		}

		public async Task<ServiceResult<List<CommonVacancyDTO>>> GetVacanciesAsync(VacancyRequestParameters requestParameters)
		{
			if (requestParameters.PageNumber == null || requestParameters.Pagesize == null)
			{
				return ServiceResult<List<CommonVacancyDTO>>.Failure(ServiceErrorType.BadModel,
					requestParameters.PageNumber == null ? "Page number is null" : "Page size is null");
			}
			var vacancies = await GetVacancies(requestParameters)
				.OrderBy(v => v.CreateDate)
				.Skip((requestParameters.PageNumber.Value - 1) * requestParameters.Pagesize.Value)
				.Take(requestParameters.Pagesize.Value)
				.Select(v => new CommonVacancyDTO
				{
					Id = v.Id,
					CategoryID = v.CategoryID,
					EmployerID = v.Employer.PublicId,
					Employer = v.Employer.CompanyName,
					CreateDate = v.CreateDate,
					Title = v.Title,
					Location = v.Location,
					Salary = v.Salary,
					PhoneNumber = v.PhoneNumber,
					EMail = v.EMail,
					Description = v.Description
				}).ToListAsync();
			return ServiceResult<List<CommonVacancyDTO>>.Success(vacancies);
		}

		private IQueryable<Vacancy> GetVacancies(VacancyRequestParameters requestParameters)
		{
			return context.Vacancies
				.Where(v => requestParameters.CategoryID == null || v.CategoryID == requestParameters.CategoryID)
				.Where(v => requestParameters.EmployerID == null || v.Employer.PublicId.ToString() == requestParameters.EmployerID);
		}
	}
}
