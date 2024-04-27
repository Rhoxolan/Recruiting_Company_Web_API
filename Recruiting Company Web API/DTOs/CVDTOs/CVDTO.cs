namespace Recruiting_Company_Web_API.DTOs.CVDTOs
{
	public class CVDTO
	{
		public required ulong? Id { get; set; }

		public required DateTime? UploadDate { get; set; }	

		public required string? File {  get; set; }

		public required string? Format { get; set; }

		public required bool? IsFile { get; set; }

		public required bool? IsLink { get; set; }

		public required string? Link { get; set; }
	}
}
