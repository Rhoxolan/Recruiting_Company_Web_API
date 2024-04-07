using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public class GuestService(ApplicationContext context) : IGuestService
	{
		public async Task<dynamic?> GetVacancyAsync(ulong id)
		{
			return await context.Vacancies
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
		}

		public async Task<int> GetVacanciesCountAsync(VacancyRequestParameters requestParameters)
		{
			return await GetVacancies(requestParameters).CountAsync();
		}

		public async Task<IEnumerable<dynamic>> GetVacanciesAsync(VacancyRequestParameters requestParameters)
		{
			_ = requestParameters.PageNumber ?? throw new Exception("Page number is null");
			_ = requestParameters.Pagesize ?? throw new Exception("Page size is null");
			return await GetVacancies(requestParameters)
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
		}

		private IQueryable<Vacancy> GetVacancies(VacancyRequestParameters requestParameters)
		{
			return context.Vacancies
				.Where(v => requestParameters.CategoryID == null || v.CategoryID == requestParameters.CategoryID)
				.Where(v => requestParameters.EmployerID == null || v.Employer.PublicId.ToString() == requestParameters.EmployerID);
		}
	}
}
