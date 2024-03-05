using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public class Vacancy
	{
		public ulong Id { get; set; }

		[MaxLength(100)]
		public required string Title { get; set; }

		public ushort? Salary { get; set; }

		[MaxLength(100)]
		public string? PhoneNumber { get; set; }

		[MaxLength(100)]
		public string? EMail { get; set; }

		[MinLength(100)]
		[MaxLength(1000)]
		public required string Description { get; set; }

		public required Category Category { get; set; }

	}
}
