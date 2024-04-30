namespace Recruiting_Company_Web_API.DTOs.Abstract
{
	public class CVDTOBase
	{
		public required bool? IsFile { get; set; }

		public required bool? IsLink { get; set; }

		public required string? Link { get; set; }
	}
}
