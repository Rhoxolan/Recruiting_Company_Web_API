using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public interface IEmployerService
	{
		Task<(bool findUserResult, Vacancy? vacancy)> AddVacancyAsync(VacancyModel model, string name);
		Task<(bool findUserResult, bool findVacancyResult)> DeleteVacancyAsync(ulong id, string name);
		Task<(bool findUserResult, Vacancy? vacancy)> EditVacansyAsync(VacancyModel model, string name);
		Task<(bool findUserResult, List<Vacancy>? vacancies)> GetVacanciesAsync(string name);
	}
}
