using Recruiting_Company_Web_API.DTOs.FileDTOs;
using Recruiting_Company_Web_API.DTOs.ResponseDTOs;
using Recruiting_Company_Web_API.DTOs.VacancyDTOs;
using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public interface IEmployerService
	{
		Task<ServiceResult<VacancyDTO>> AddVacancyAsync(VacancyModel model, string name);
		Task<ServiceResult> DeleteVacancyAsync(ulong id, string name);
		Task<ServiceResult<VacancyDTO>> EditVacansyAsync(VacancyModel model, string name);
		Task<ServiceResult<List<VacancyDTO>>> GetVacanciesAsync(string name);
		Task<ServiceResult<FileDTO>> GetVacancyResponseCVFileAsync(ulong id, string name);
		Task<ServiceResult<List<ResponseDTO>>> GetVacancyResponsesAsync(ulong id, string name);
	}
}
