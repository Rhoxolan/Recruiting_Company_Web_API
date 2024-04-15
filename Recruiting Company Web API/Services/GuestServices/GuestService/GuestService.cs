using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public class GuestService(ApplicationContext context) : IGuestService
	{
		public async Task<ServiceResult<dynamic>> GetVacancyAsync(ulong id)
		{
			var vacancy = await context.Vacancies
				.Where(v => v.Id == id)
				.Select(v => new
				{
					v.Id,
					v.CategoryID,
					EmployerID = v.Employer.PublicId,
					Employer = v.Employer.CompanyName,
					v.CreateDate,
					v.Title,
					v.Location,
					v.Salary,
					v.PhoneNumber,
					v.EMail,
					v.Description
				}).FirstOrDefaultAsync();
			if (vacancy == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.EntityNotFound);
			}
			return ServiceResult<dynamic>.Success(vacancy);
		}

		public async Task<ServiceResult<int>> GetVacanciesCountAsync(VacancyRequestParameters requestParameters)
		{
			var vacanciesCount = await GetVacancies(requestParameters).CountAsync();
			return ServiceResult<int>.Success(vacanciesCount);
		}

		public async Task<ServiceResult<IEnumerable<dynamic>>> GetVacanciesAsync(VacancyRequestParameters requestParameters)
		{
			if (requestParameters.PageNumber == null || requestParameters.Pagesize == null)
			{
				return ServiceResult<IEnumerable<dynamic>>.Failure(ServiceErrorType.BadModel,
					requestParameters.PageNumber == null ? "Page number is null" : "Page size is null");
			}
			var vacancies = await GetVacancies(requestParameters)
				.OrderBy(v => v.CreateDate)
				.Skip((requestParameters.PageNumber.Value - 1) * requestParameters.Pagesize.Value)
				.Take(requestParameters.Pagesize.Value)
				.Select(v => new
				{
					v.Id,
					v.CategoryID,
					EmployerID = v.Employer.PublicId,
					Employer = v.Employer.CompanyName,
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

		private IQueryable<Vacancy> GetVacancies(VacancyRequestParameters requestParameters)
		{
			return context.Vacancies
				.Where(v => requestParameters.CategoryID == null || v.CategoryID == requestParameters.CategoryID)
				.Where(v => requestParameters.EmployerID == null || v.Employer.PublicId.ToString() == requestParameters.EmployerID);
		}
	}
}
