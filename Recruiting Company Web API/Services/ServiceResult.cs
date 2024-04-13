namespace Recruiting_Company_Web_API.Services
{
	public class ServiceResult : ServiceResultBase
	{
		public static ServiceResult Failure(ServiceErrorType type, string? message = null)
		{
			return new ServiceResult
			{
				Error = new ServiceError
				{
					ErrorType = type,
					Message = message
				}
			};
		}

		public static ServiceResult Success()
		{
			return new ServiceResult
			{
				Succeded = true
			};
		}
	}
}
