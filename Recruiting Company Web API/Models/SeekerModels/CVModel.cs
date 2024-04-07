using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Models.SeekerModels
{
	public class CVModel
	{
		[Base64String]
		public string? File {  get; set; }

		[MaxLength(3)]
		public string? FileFormat { get; set; }

		[MaxLength(100)]
		[Url]
		public string? Link { get; set; }
	}
}
