namespace Recruiting_Company_Web_API.DTOs.ResponseDTOs
{
	public class OwnResponseDTO : ResponseDTO
	{
		public required ulong? VacancyID { get; set; }

		public required ulong? CVId { get; set; }

		public required DateTime? CVUploadDate { get; set; }
	}
}
