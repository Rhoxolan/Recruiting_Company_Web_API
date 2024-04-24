namespace Recruiting_Company_Web_API.DTOs.VacancyDTOs
{
	public class CommonVacancyDTO : VacancyDTO
	{
		public required Guid? EmployerID { get; set; }

		public required string? Employer {  get; set; }
	}
}
