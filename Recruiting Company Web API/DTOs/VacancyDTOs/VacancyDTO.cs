namespace Recruiting_Company_Web_API.DTOs.VacancyDTOs
{
	public class VacancyDTO
	{
		public required ulong? Id { get; set; }

		public required short? CategoryID { get; set; }

		public required DateTime? CreateDate { get; set; }

		public required string? Title { get; set; }

		public required string? Location { get; set; }

		public required int? Salary { get; set; }

		public required string? PhoneNumber { get; set; }

		public required string? EMail { get; set; }

		public required string? Description { get; set;}
	}
}
