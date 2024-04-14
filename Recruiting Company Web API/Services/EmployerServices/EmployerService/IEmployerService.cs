using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public interface IEmployerService
	{
		Task<ServiceResult<dynamic>> AddVacancyAsync(VacancyModel model, string name);
		Task<ServiceResult> DeleteVacancyAsync(ulong id, string name);
		Task<ServiceResult<dynamic>> EditVacansyAsync(VacancyModel model, string name);
		Task<ServiceResult<IEnumerable<dynamic>>> GetVacanciesAsync(string name);
		Task<ServiceResult<dynamic>> GetVacancyResponseCVFileAsync(ulong id, string name);
		Task<ServiceResult<IEnumerable<dynamic>>> GetVacancyResponsesAsync(ulong id, string name);
	}
}
