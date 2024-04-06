namespace Recruiting_Company_Web_API.Entities
{
	public class Response
	{
		public ulong Id { get; set; }

		public required DateTime ResponseTime { get; set; }

		public required Vacancy Vacancy { get; set; }

		public ulong? VacancyID { get; set; }

		public required CV CV { get; set; }

		public ulong? CVID {  get; set; }
	}
}
