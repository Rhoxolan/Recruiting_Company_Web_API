using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public class CV
	{
		public ulong Id { get; set; }

		public required DateTime UploadDate { get; set; }

		[MaxLength(10 * 1024 * 1024)]
		public byte[]? File {  get; set; }

		[MaxLength(3)]
		public string? FileFormat { get; set; }

		[MaxLength(100)]
		public string? Link { get; set; }

		public required Seeker Seeker { get; set; }

		public string? SeekerID { get; set; }
	}
}
