using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.ResponseService
{
	public interface IResponseService
	{
		Task<ServiceResult<bool>> CheckIsRespondedAsync(ulong vacancyId, string name);
		Task<ServiceResult<IEnumerable<dynamic>>> GetResponsesAsync(string name);
		Task<ServiceResult<dynamic>> RespondToVacancyAsync(ResponseModel model, string name);
	}
}
