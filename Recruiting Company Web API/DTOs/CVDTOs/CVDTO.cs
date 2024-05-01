using Recruiting_Company_Web_API.DTOs.Abstract;

namespace Recruiting_Company_Web_API.DTOs.CVDTOs
{
	public class CVDTO : CVDTOBase
	{
		public required ulong? Id { get; set; }

		public required DateTime? UploadDate { get; set; }
	}
}
