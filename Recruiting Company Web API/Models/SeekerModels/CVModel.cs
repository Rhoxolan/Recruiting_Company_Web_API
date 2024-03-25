using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Models.SeekerModels
{
	public class CVModel
	{
		[RegularExpression("/^[a-z0-9.]+$/i")]
		public string? File {  get; set; }

		[MaxLength(3)]
		public string? FileFormat { get; set; }

		[MaxLength(100)]
		[Url]
		public string? Link { get; set; }
	}
}
