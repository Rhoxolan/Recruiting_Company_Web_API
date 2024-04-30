using Recruiting_Company_Web_API.DTOs.CVDTOs;
using Recruiting_Company_Web_API.DTOs.FileDTOs;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.CVService
{
	public interface ICVService
	{
		Task<ServiceResult<List<CVDTO>>> GetCVsAsync(string name);
		Task<ServiceResult<CVDTO>> UploadCVAsync(CVModel model, string name);
		Task<ServiceResult> DeleteCVAsync(ulong id, string name);
		Task<ServiceResult<FileDTO>> GetCVsFileAsync(ulong id, string name);
	}
}
