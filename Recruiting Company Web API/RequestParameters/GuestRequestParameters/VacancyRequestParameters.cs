using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters
{
	public class VacancyRequestParameters
	{
		[Required]
		public int PageNumber { get; set; }

		[Required]
		[Range(1, 100)]
		public short Pagesize { get; set; }

		public short? CategoryID { get; set; }
	}
}
