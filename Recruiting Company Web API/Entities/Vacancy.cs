using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public class Vacancy
	{
		public ulong Id { get; set; }

		public required DateTime CreateDate { get; set; }

		[MaxLength(100)]
		public required string Title { get; set; }

		[MaxLength(50)]
		public string? Location { get; set; }

		public int? Salary { get; set; }

		[MaxLength(100)]
		public string? PhoneNumber { get; set; }

		[MaxLength(100)]
		public string? EMail { get; set; }

		[MinLength(100)]
		[MaxLength(1000)]
		public required string Description { get; set; }

		public required Category Category { get; set; }

		public short? CategoryID { get; set; }

		public required Employer Employer { get; set; }

		public string? EmployerID { get; set; }

	}
}
