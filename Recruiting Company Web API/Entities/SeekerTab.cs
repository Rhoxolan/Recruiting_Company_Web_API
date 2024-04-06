namespace Recruiting_Company_Web_API.Entities
{
	public class SeekerTab
	{
		public int Id { get; set; }

		public required Seeker Seeker { get; set; }

		public string? SeekerID { get; set; }

		public required Vacancy Vacancy { get; set; }

		public ulong? VacancyID { get; set; }
	}
}
