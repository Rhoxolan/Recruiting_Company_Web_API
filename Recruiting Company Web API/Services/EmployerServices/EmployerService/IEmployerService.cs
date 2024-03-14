using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public interface IEmployerService
	{
		Task<(bool findUserResult, dynamic? vacancy)> AddVacancyAsync(VacancyModel model, string name);
		Task<(bool findUserResult, bool findVacancyResult)> DeleteVacancyAsync(ulong id, string name);
		Task<(bool findUserResult, dynamic? vacancy)> EditVacansyAsync(VacancyModel model, string name);
		Task<(bool findUserResult, IEnumerable<dynamic>? vacancies)> GetVacanciesAsync(string name);
		Task<(bool findUserResult, dynamic? responseCVFile)> GetVacancyResponseCVFileAsync(ulong id, string name);
		Task<(bool findUserResult, bool findVacancyResult, IEnumerable<dynamic>? responses)> GetVacancyResponsesAsync(ulong id, string name);
	}
}
