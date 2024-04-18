namespace Recruiting_Company_Web_API.Services
{
	public abstract class ServiceResultBase
	{
		public ServiceError Error { get; init; } = new();

		public bool Succeded { get; init; } = false;
	}
}
