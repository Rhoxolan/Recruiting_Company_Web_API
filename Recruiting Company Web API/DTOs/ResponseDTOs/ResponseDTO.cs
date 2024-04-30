using Recruiting_Company_Web_API.DTOs.Abstract;

namespace Recruiting_Company_Web_API.DTOs.ResponseDTOs
{
	public class ResponseDTO : CVDTOBase
	{
		public required ulong? Id { get; set; }

		public required DateTime? ResponseTime { get; set; }
	}
}
