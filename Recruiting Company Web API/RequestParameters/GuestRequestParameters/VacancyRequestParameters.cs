using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters
{
	public class VacancyRequestParameters
	{
		[Range(1, int.MaxValue)]
		public int? PageNumber { get; set; }

		[Range(1, 100)]
		public short? Pagesize { get; set; }

		public short? CategoryID { get; set; }

		public string? EmployerID { get; set; }
	}
}
