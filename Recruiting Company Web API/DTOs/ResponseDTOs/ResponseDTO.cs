namespace Recruiting_Company_Web_API.DTOs.ResponseDTOs
{
	public class ResponseDTO
	{
		public required ulong? Id { get; set; }

		public required DateTime? ResponseTime { get; set; }

		public required bool? IsLink { get; set; }

		public required bool? IsFile { get; set; }

		public required string? Link { get; set; }
	}
}
