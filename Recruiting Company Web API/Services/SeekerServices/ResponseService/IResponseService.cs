using Recruiting_Company_Web_API.DTOs.FileDTOs;
using Recruiting_Company_Web_API.DTOs.ResponseDTOs;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.ResponseService
{
	public interface IResponseService
	{
		Task<ServiceResult<bool>> CheckIsRespondedAsync(ulong vacancyId, string name);
		Task<ServiceResult<FileDTO>> GetResponseCVFileAsync(ulong responseId, string name);
		Task<ServiceResult<List<OwnResponseDTO>>> GetResponsesAsync(string name);
		Task<ServiceResult<OwnResponseDTO>> RespondToVacancyAsync(ResponseModel model, string name);
	}
}
