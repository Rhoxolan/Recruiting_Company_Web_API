using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.ResponseService
{
	public interface IResponseService
	{
		Task<(bool findUserResult, bool isResponded)> CheckIsRespondedAsync(ulong vacancyId, string name);
		Task<(bool findUserResult, IEnumerable<dynamic>? responses)> GetResponsesAsync(string name);
		Task<(bool findUserResult, bool findVacancyResult, bool findCVResult, dynamic? response)> RespondToVacancyAsync(ResponseModel model, string name);
	}
}
