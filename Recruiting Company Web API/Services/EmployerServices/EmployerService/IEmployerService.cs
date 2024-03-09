using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public interface IEmployerService
	{
		Task<List<Vacancy>> GetVacanciesAsync(Employer employer);

		Task<Employer?> GetEmployerAsync(string name);

		Task<Vacancy> AddVacancyAsync(VacancyModel model, Employer employer);
	}
}
