using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public interface IEmployerService
	{
		Task<(bool findUserResult, object? vacancy)> AddVacancyAsync(VacancyModel model, string name);
		Task<(bool findUserResult, bool findVacancyResult)> DeleteVacancyAsync(ulong id, string name);
		Task<(bool findUserResult, object? vacancy)> EditVacansyAsync(VacancyModel model, string name);
		Task<(bool findUserResult, IEnumerable<object>? vacancies)> GetVacanciesAsync(string name);
		Task<(bool findUserResult, object? responseCVFile)> GetVacancyResponseCVFileAsync(ulong id, string name);
		Task<(bool findUserResult, bool findVacancyResult, IEnumerable<object>? responses)> GetVacancyResponsesAsync(ulong id, string name);
	}
}
