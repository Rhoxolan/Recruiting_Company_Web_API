using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Models.SeekerModels
{
	public class CVModel
	{
		[RegularExpression("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$")]
		public string? File {  get; set; }

		[MaxLength(3)]
		public string? FileFormat { get; set; }

		[MaxLength(100)]
		[Url]
		public string? Link { get; set; }
	}
}
