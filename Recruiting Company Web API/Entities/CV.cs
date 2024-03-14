using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public record CV
	{
		public ulong Id { get; init; }

		public required DateTime UploadDate { get; init; }

		[MaxLength(10 * 1024 * 1024)]
		public byte[]? File {  get; init; }

		[MaxLength(3)]
		public string? FileFormat { get; init; }

		[MaxLength(100)]
		public string? Link { get; init; }

		public required Seeker Seeker { get; init; }
	}
}
